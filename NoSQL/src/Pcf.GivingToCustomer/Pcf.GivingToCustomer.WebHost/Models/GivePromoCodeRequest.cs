using System;

namespace Pcf.GivingToCustomer.WebHost.Models
{
    public class GivePromoCodeRequest
    {
        /// <example>Кампания совместно с журналом Афиша</example>
        public string ServiceInfo { get; set; }

        /// <example>513B2043-F1B2-44CF-8935-52E7604EA88A</example>
        public Guid PartnerId { get; set; }

        /// <example>95C6CF0D-3A4E-4D85-ACCB-4D487D1FBA36</example>
        public Guid PromoCodeId { get; set; }

        /// <example>OtusTeatr15</example>
        public string PromoCode { get; set; }

        /// <example>ef7f299f-92d7-459f-896e-078ed53ef99c</example>
        public Guid PreferenceId { get; set; }

        /// <example>01-01-2024</example>
        public string BeginDate { get; set; }

        /// <example>31-12-2024</example>
        public string EndDate { get; set; }
    }
}
