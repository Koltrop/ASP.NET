using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Pcf.Administration.Core.Domain
{
    public class BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
    }
}