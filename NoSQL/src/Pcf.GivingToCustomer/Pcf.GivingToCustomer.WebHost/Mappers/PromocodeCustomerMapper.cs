using Pcf.GivingToCustomer.Core.Domain;

namespace Pcf.GivingToCustomer.WebHost.Mappers;

public class PromocodeCustomerMapper
{
    public static PromoCodeCustomer MapFromModel(PromoCode promoCode, Customer customer)
        => new()
        {
            PromoCodeId = promoCode.Id,
            PromoCode = promoCode,
            CustomerId = customer.Id,
            Customer = customer
        };
}
