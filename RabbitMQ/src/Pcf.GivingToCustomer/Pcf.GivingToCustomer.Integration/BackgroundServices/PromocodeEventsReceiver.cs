using Microsoft.Extensions.Hosting;
using Pcf.Core.Integration;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Abstractions.Services;
using Pcf.GivingToCustomer.Core.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Integration.BackgroundServices;

public sealed class PromocodeEventsReceiver(
    ConnectionFactory factory,
    IPromocodeService service) : BackgroundService
{
    private readonly ConnectionFactory _factory = factory;
    private readonly IPromocodeService _service = service;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Wait for RabbitMQ to start
        await Task.Delay(3000, stoppingToken);

        Console.WriteLine("PromocodeEventsReceiver is starting.");

        using var connection = await _factory.CreateConnectionAsync(stoppingToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.ExchangeDeclareAsync(RabbitEventsConstants.PromocodeExchangeKey, 
            ExchangeType.Topic, 
            durable: true, 
            cancellationToken: stoppingToken);

        var queue = await channel.QueueDeclareAsync("giving-to-customer-promocode-on-creating",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        await channel.QueueBindAsync(queue.QueueName,
            RabbitEventsConstants.PromocodeExchangeKey,
            "created.*",
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            Console.WriteLine("ReceivedAsync in a consumer");

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var dto = JsonSerializer.Deserialize<PromocodeDto>(message);

            if (dto != null)
            {
                try
                {
                    await _service.GivePromoCodesToCustomersWithPreference(dto);
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine($"Error on processing message {message}. {ex.Message}");
                    return;
                }
            }
            else
            {
                Console.WriteLine("PromocodeDto is null");
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
