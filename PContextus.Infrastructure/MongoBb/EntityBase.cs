using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PContextus.Infrastructure.MongoDb
{
    public abstract class EntityBase
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
