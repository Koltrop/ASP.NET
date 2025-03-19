using Pcf.Core.Integration;
using Pcf.ReceivingFromPartner.Core.Domain;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Integration;

public sealed class RabbitEventService(ConnectionFactory factory)
{
    private readonly ConnectionFactory factory = factory;

    public async Task SendPromocodeCreatedEvent(PromoCode promocode)
    {
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();
        
        await channel.ExchangeDeclareAsync(RabbitEventsConstants.PromocodeExchangeKey,
            ExchangeType.Topic,
            durable: true);

        var dto = new PromocodeDto
        {
            Code = promocode.Code,
            ServiceInfo = promocode.ServiceInfo,
            BeginDate = promocode.BeginDate,
            EndDate = promocode.EndDate,
            PartnerId = promocode.PartnerId,
            PreferenceId = promocode.PreferenceId,
            PartnerManagerId = promocode.PartnerManagerId
        };
        var message = JsonSerializer.Serialize(dto);
        var body = Encoding.UTF8.GetBytes(message);
        var managerKeyPart = promocode.PartnerManagerId.HasValue ? "withmanager" : "withoutmanager";
        var routingKey = "created." + managerKeyPart;
        await channel.BasicPublishAsync(RabbitEventsConstants.PromocodeExchangeKey, routingKey, body);
    }
}
