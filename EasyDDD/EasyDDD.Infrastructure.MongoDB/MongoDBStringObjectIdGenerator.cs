using EasyDDD.Core.IdGenerator;
using MongoDB.Bson;

namespace EasyDDD.Infrastructure.Data.MongoDB
{
    public class MongoDBStringObjectIdGenerator : IIdentityGenerator
    {
        public string NewId()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}
