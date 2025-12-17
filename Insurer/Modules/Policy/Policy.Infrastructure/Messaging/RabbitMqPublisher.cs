using System.Text;
using System.Text.Json;
using Policy.Infrastructure.Interfaces;
using RabbitMQ.Client;
using Shared;

namespace Policy.Infrastructure.Messaging;

public class RabbitMqPublisher(IConnection connection) : IEventPublisher
{
    public async Task PublishAsync(BaseEvent @event, string queueName)
    {
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            autoDelete: false,
            exclusive: false,
            arguments: null);

        var json = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: body);
    }
}