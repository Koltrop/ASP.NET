using System;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement;
public sealed class CustomerPreference
{
    public Guid CustomerId { get; set; }

    public Guid PreferenceId { get; set; }
}

