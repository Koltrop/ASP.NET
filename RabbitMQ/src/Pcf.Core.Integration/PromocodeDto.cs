namespace Pcf.Core.Integration;

public sealed class PromocodeDto
{
    public string? Code { get; set; }

    public string? ServiceInfo { get; set; }

    public DateTime BeginDate { get; set; }

    public DateTime EndDate { get; set; }

    public Guid PartnerId { get; set; }

    public Guid? PartnerManagerId { get; set; }

    public Guid PreferenceId { get; set; }
}

