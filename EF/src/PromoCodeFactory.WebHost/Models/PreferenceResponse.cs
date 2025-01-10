using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;

namespace PromoCodeFactory.WebHost.Models;

public sealed class PreferenceResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int PromocodeValidityInDays { get; set; }

    public PreferenceResponse(Preference source)
    {
        Id = source.Id;
        Name = source.Name;
        PromocodeValidityInDays = source.PromocodeValidityInDays;
    }
}
