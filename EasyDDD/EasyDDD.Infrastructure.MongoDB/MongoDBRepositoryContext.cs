using EasyDDD.Core;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace EasyDDD.Infrastructure.Data.MongoDB
{
    public class MongoDBRepositoryContext : RepositoryContext, IMongoDBRepositoryContext
    {
        private readonly Guid id = Guid.NewGuid();
        private readonly MongoServer _server;
        private readonly MongoDatabase _database;
        private readonly MongoServerSettings _mongoServerSettings;
        private readonly object syncObj = new object();
        private readonly Dictionary<Type, MongoCollection> _mongoCollections = new Dictionary<Type, MongoCollection>();
        private readonly IMapping _mapping;
        /// <summary>
        /// 连接字符串名称
        /// </summary>
        /// <param name="name"></param>
        public MongoDBRepositoryContext(string name)
        {

            MongoDBBootstrapper.Init();
            var connectionString = GetConnectionStringByName(name);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(name, "The mongodb connection string is empty.");
            }

            var mongoUrl = new MongoUrl(connectionString);
            _mongoServerSettings = MongoServerSettings.FromUrl(mongoUrl);
            _server = new MongoServer(_mongoServerSettings);
            _database = _server.GetDatabase(mongoUrl.DatabaseName);
            _mapping = IoC.Resolve<IMapping>();
        }

        public MongoDatabase MongoDatabase
        {
            get { return _database; }
        }
            

        public MongoCollection GetCollectionForType(Type type)
        {
           
            lock (syncObj)
            {

                if (this._mongoCollections.ContainsKey(type))
                    return this._mongoCollections[type];
                else
                {
                    string collectionName = string.Empty;
                    if (_mapping != null)
                    {
                        var mappings = _mapping.GetEntityVsTableNameMappingInfos();
                        if (mappings.ContainsKey(type))
                        {
                            collectionName = _mapping.GetEntityVsTableNameMappingInfos()[type];
                        }
                    }

                    MongoCollection mongoCollection = null;
                    if (string.IsNullOrEmpty(collectionName))
                    {
                        mongoCollection = this._database.GetCollection(type.Name);
                    }
                    else
                    {
                        mongoCollection = this._database.GetCollection(collectionName);
                    }

                    this._mongoCollections.Add(type, mongoCollection);
                    return mongoCollection;
                }
            }
        }


        /// <summary>
        /// 不支持分布式事物
        /// </summary>
        public override bool DistributedTransactionSupported
        {
            get { return false; }
        }

        public MongoServerSettings ServerSettings
        {
            get { return _mongoServerSettings; }
        }

        public MongoDatabase Database
        {
            get { return _database; }
        }

        public MongoServer Server
        {
            get { return _server; }
        }

        public override void Commit()
        {
            lock (syncObj)
            {
                foreach (var newObj in _localNewCollection.Value)
                {
                    MongoCollection collection = this.GetCollectionForType(newObj.Value.GetType());
                    collection.Insert(newObj.Value);
                }
                foreach (var modifiedObj in this._localModifiedCollection.Value)
                {
                    MongoCollection collection = this.GetCollectionForType(modifiedObj.Value.GetType());
                    collection.Save(modifiedObj.Value);
                }
                foreach (var delObj in this._localDeletedCollection.Value)
                {
                    Type objType = delObj.Value.GetType();
                    PropertyInfo propertyInfo = objType.GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (propertyInfo == null)
                        throw new InvalidOperationException("Cannot delete an object which doesn't contain an ID property.");
                    string id = (string)propertyInfo.GetValue(delObj.Value, null);
                    MongoCollection collection = this.GetCollectionForType(objType);
                    IMongoQuery query = Query.EQ("_id", id);
                    collection.Remove(query);
                }
                base.ClearRegistrations();
                this.Committed = true;
            }
        }

        public override void Rollback()
        {
            this.Committed = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _server.Disconnect();
            }
            base.Dispose(disposing);
        }



        #region Public Static Methods
        /// <summary>
        /// Registers the MongoDB Bson serialization conventions.
        /// </summary>
        /// <param name="additionConventions">Additional conventions that needs to be registered.</param>
        public static void RegisterConventions(IEnumerable<IConvention> additionConventions = null)
        {
            var conventionPack = new ConventionPack();
            conventionPack.Add(new NamedIdMemberConvention("id", "_id"));
            conventionPack.Add(new IgnoreExtraElementsConvention(true));
            conventionPack.Add(new StringObjectIdIdGeneratorConvention());
            //枚举序列化为字符串
            conventionPack.Add(new EnumRepresentationConvention(BsonType.String));

            if (additionConventions != null)
                conventionPack.AddRange(additionConventions);

            ConventionRegistry.Register("DefaultConvention", conventionPack, t => true);
        }


        public static string GetConnectionStringByName(string name)
        {
            var connectionString = "";
            var settings = ConfigurationManager.ConnectionStrings[name];
            if (settings != null)
            {
                connectionString = settings.ConnectionString;
            }
            return connectionString;
        }

        #endregion
    }
}
