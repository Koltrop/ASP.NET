using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Pcf.GivingToCustomer.Core.Domain
{
    public class Customer
        : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        [BsonIgnore]
        public virtual ICollection<CustomerPreference> CustomerPreferences { get; set; }
        
        public ICollection<Preference> Preferences { get; set; }

        [BsonIgnore]
        public virtual ICollection<PromoCodeCustomer> PromoCodes { get; set; }
    }
}