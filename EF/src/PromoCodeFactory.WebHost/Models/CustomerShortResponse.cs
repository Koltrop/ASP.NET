using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;

namespace PromoCodeFactory.WebHost.Models
{
    public class CustomerShortResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public CustomerShortResponse(Customer source)
        {
            Id = source.Id;
            FirstName = source.FirstName;
            LastName = source.LastName;
            Email = source.Email;
        }
    }
}