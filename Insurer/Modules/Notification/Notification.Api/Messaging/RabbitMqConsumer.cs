using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Notification.Api.Data;
using Notification.Api.Entities;
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
    private IChannel _channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        try
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    if (ea.BasicProperties?.Headers == null ||
                        !ea.BasicProperties.Headers.TryGetValue("eventType", out var rawType))
                    {
                        logger.LogError("Missing eventType header");
                        return;
                    }

                    var eventType = Encoding.UTF8.GetString((byte[])rawType!);
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var baseEvent = JsonSerializer.Deserialize<BaseEvent>(message);

                    using var scope = serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

                    if (await db.ProcessedEvents.AnyAsync(e => e.EventId == baseEvent!.EventId, stoppingToken))
                    {
                        logger.LogInformation($"Event {baseEvent!.EventId} already processed");

                        await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                        return;
                    }

                    var handlers = scope.ServiceProvider.GetRequiredService<IEnumerable<IEventHandler>>();

                    var handler = handlers
                        .FirstOrDefault(h => h.EventType == eventType);

                    if (handler == null)
                    {
                        logger.LogError("Handler not found");
                        await _channel.BasicAckAsync(ea.DeliveryTag, false);
                        return;
                    }

                    await handler.HandleAsync(message, stoppingToken);

                    var processedEvent = new ProcessedEvent
                    {
                        EventId = baseEvent!.EventId,
                        ProcessedAt = DateTime.UtcNow,
                    };

                    db.ProcessedEvents.Add(processedEvent);
                    await db.SaveChangesAsync(stoppingToken);

                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Message processing error");

                    var retryCount = RetryCount(ea);

                    if (retryCount >= _queueOptions.MaxRetryCount)
                    {
                        await PublishToDlqAsync(_channel, ea, stoppingToken);
                    }
                    else
                    {
                        await PublishToRetryAsync(_channel, ea, retryCount, stoppingToken);
                    }
                }
            };

            await _channel.BasicConsumeAsync(
                queue: _queueOptions.PolicyCreated,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken
            );

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "Message processing cancelled");
        }
    }

    private int RetryCount(BasicDeliverEventArgs ea)
    {
        if (ea.BasicProperties?.Headers == null)
            return 0;

        return ea.BasicProperties.Headers.TryGetValue("x-retry-count", out var count)
            ? Convert.ToInt32(count)
            : 0;
    }

    private async Task PublishToRetryAsync(
        IChannel channel,
        BasicDeliverEventArgs ea,
        int retryCount,
        CancellationToken stoppingToken)
    {
        var props = new BasicProperties
        {
            Headers = (ea.BasicProperties?.Headers != null
                ? ea.BasicProperties.Headers.ToDictionary(x => x.Key, x => x.Value)!
                : new Dictionary<string, object>())!,
        };

        props.Headers["x-retry-count"] = retryCount + 1;

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: _queueOptions.PolicyCreatedRetry,
            mandatory: false,
            basicProperties: props,
            body: ea.Body,
            cancellationToken: stoppingToken);
    }

    private async Task PublishToDlqAsync(
        IChannel channel,
        BasicDeliverEventArgs ea,
        CancellationToken stoppingToken)
    {
        var props = new BasicProperties
        {
            Headers = (ea.BasicProperties?.Headers != null
                ? ea.BasicProperties.Headers.ToDictionary(x => x.Key, x => x.Value)!
                : new Dictionary<string, object>())!,
        };

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: _queueOptions.PolicyCreatedDlq,
            mandatory: false,
            basicProperties: props,
            body: ea.Body,
            cancellationToken: stoppingToken);
    }


    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null)
        {
            await _channel.CloseAsync(cancellationToken);
            await _channel.DisposeAsync();
        }

        await base.StopAsync(cancellationToken);
    }
}