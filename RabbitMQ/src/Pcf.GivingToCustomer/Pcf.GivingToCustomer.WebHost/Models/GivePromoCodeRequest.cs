using Pcf.Core.Integration;
using System;

namespace Pcf.GivingToCustomer.WebHost.Models
{
    public class GivePromoCodeRequest
    {
        public string ServiceInfo { get; set; }

        public Guid PartnerId { get; set; }

        public Guid PromoCodeId { get; set; }

        public string PromoCode { get; set; }

        public Guid PreferenceId { get; set; }

        public string BeginDate { get; set; }

        public string EndDate { get; set; }

        public PromocodeDto ToPromocodeDto()
        {
            return new ()
            {
                ServiceInfo = ServiceInfo,
                PartnerId = PartnerId,
                PreferenceId = PreferenceId,
                BeginDate = DateTime.Parse(BeginDate),
                EndDate = DateTime.Parse(EndDate)
            };
        }
    }
}