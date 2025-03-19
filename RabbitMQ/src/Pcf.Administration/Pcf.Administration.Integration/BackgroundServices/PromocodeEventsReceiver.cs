using Microsoft.Extensions.Hosting;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Abstractions.Services;
using Pcf.Administration.Core.Domain.Administration;
using Pcf.Core.Integration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Pcf.Administration.Integration.BackgroundServices;

public sealed class PromocodeEventsReceiver(ConnectionFactory factory, 
    IPromocodeService promocodeService) : BackgroundService
{
    private readonly ConnectionFactory _factory = factory;
    private readonly IPromocodeService promocodeService = promocodeService;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Wait for RabbitMQ to start
        await Task.Delay(3000, stoppingToken);

        Console.WriteLine("PromocodeEventsReceiver is starting.");

        using var connection = await _factory.CreateConnectionAsync(stoppingToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        
        await channel.ExchangeDeclareAsync(RabbitEventsConstants.PromocodeExchangeKey,
            ExchangeType.Topic,
            durable: true);
        
        var queue = await channel.QueueDeclareAsync("administration-update-applied-promocodes",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        await channel.QueueBindAsync(queue.QueueName, 
            RabbitEventsConstants.PromocodeExchangeKey,
            "created.withmanager",
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            Console.WriteLine("ReceivedAsync in a consumer");

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var dto = JsonSerializer.Deserialize<PromocodeDto>(message);

            if (dto != null && dto.PartnerManagerId.HasValue)
            {
                try
                {
                    await promocodeService.UpdateAppliedPromocodesAsync(dto.PartnerManagerId.Value);

                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine($"Employee with id {dto.PartnerManagerId.Value} not found.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("PromocodeDto is null or PartnerManagerId is null. Dto: " + message);
            }

            await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            Console.WriteLine("Acknowledged message: " + ea.DeliveryTag);
        };

        var consumerTag = await channel.BasicConsumeAsync(queue.QueueName, autoAck: false, consumer, cancellationToken: stoppingToken);
        Console.WriteLine($"Consumer is set up and waiting for messages. Consumer tag: {consumerTag}");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }

    }
}
