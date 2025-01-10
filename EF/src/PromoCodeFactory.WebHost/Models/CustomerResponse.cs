using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromoCodeFactory.WebHost.Models
{
    public class CustomerResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //TODO: Добавить список предпочтений
        public List<PreferenceResponse> Preferences { get; set; }
        public List<PromoCodeShortResponse> PromoCodes { get; set; }
        public CustomerResponse(Customer source)
        {
            Id = source.Id;
            FirstName = source.FirstName;
            LastName = source.LastName;
            Email = source.Email;
            Preferences = source.Preferences
                                .Select(x => new PreferenceResponse(x))
                                .ToList();
            PromoCodes = source.Promocodes
                               .Select(x => new PromoCodeShortResponse(x))
                               .ToList();
        }
    }
}