using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace Pcf.GivingToCustomer.Core.Domain
{
    public class BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
    }
}