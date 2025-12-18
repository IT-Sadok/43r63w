using System.Text;
using System.Text.Json;
using Policy.Infrastructure.Interfaces;
using RabbitMQ.Client;
using Shared;

namespace Policy.Infrastructure.Messaging;

public class RabbitMqPublisher(IConnection connection) : IEventPublisher
{
    public async Task PublishAsync<T>(T @event, string queueName,CancellationToken cancellationToken)
    {
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            autoDelete: false,
            exclusive: false,
            arguments: null, 
            cancellationToken: cancellationToken);

        var json = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: body, 
            cancellationToken: cancellationToken);
    }
}