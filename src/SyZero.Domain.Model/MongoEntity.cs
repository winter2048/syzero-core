using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SyZero.Domain.Model
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public class MongoEntity : IMongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
