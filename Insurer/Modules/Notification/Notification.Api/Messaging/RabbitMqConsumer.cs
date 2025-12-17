using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;

namespace Notification.Api.Messaging;

public class RabbitMqConsumer(
    IConnection connection,
    IOptions<RabbitMqQueue> rabbitMqQueueOptions) : BackgroundService
{
    private readonly RabbitMqQueue _queueOptions = rabbitMqQueueOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: _queueOptions.PolicyCreated, 
            durable: true, 
            exclusive: false,
            autoDelete: false, 
            arguments: null, 
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += (model, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            var baseEvent = JsonSerializer.Deserialize<BaseEvent>(message);

            if (baseEvent?.EventType == "PolicyCreatedEvent")
            {
                Console.WriteLine($"Event type : {baseEvent.EventType},consumed,policy number : ");
            }

            return Task.CompletedTask;
        };
        
        await channel.BasicConsumeAsync(
            queue: _queueOptions.PolicyCreated,
            autoAck: true, 
            consumer: consumer,
            cancellationToken: stoppingToken
        );
    }
}