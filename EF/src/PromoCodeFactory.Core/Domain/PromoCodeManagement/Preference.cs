using System.Collections.Generic;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Preference
        : BaseEntity
    {
        public string Name { get; set; }
        public int PromocodeValidityInDays { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}