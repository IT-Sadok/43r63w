using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Notification.Api.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;

namespace Notification.Api.Messaging;

public class RabbitMqConsumer(
    IConnection connection,
    IOptions<RabbitMqQueue> rabbitMqQueueOptions,
    ILogger<RabbitMqConsumer> logger,
    IServiceProvider serviceProvider) : BackgroundService
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

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                var baseEvent = JsonSerializer.Deserialize<BaseEvent>(message);
                
                logger.LogInformation(baseEvent.EventType);

                if (baseEvent == null)
                {
                    logger.LogWarning("Base event is null");
                    return;
                }

                using var scope = serviceProvider.CreateScope();

                var handlers = scope.ServiceProvider.GetRequiredService<IEnumerable<IEventHandler>>();

                var handler = handlers
                    .FirstOrDefault(h => h.EventType == baseEvent?.EventType);

                if (handler == null)
                {
                    logger.LogError("Handler not found");
                    return;
                }

                await handler.HandleAsync(message, stoppingToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        };

        await channel.BasicConsumeAsync(
            queue: _queueOptions.PolicyCreated,
            autoAck: true,
            consumer: consumer,
            cancellationToken: stoppingToken
        );
    }
}