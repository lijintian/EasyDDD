using EasyDDD.Core.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDDD.Infrastructure.Data.MongoDB
{
    public interface IMongoDBRepositoryContext : IRepositoryContext
    {
        /// <summary>
        /// Gets the <see cref="MongoCollection"/> instance by the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> object.</param>
        /// <returns>The <see cref="MongoCollection"/> instance.</returns>
        MongoCollection GetCollectionForType(Type type);

        MongoDatabase MongoDatabase { get; }

    }
}
