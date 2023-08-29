using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.Common
{
    public class BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
