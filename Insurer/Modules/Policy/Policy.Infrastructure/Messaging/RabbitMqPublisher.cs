using Policy.Infrastructure.Interfaces;
using RabbitMQ.Client;

namespace Policy.Infrastructure.Messaging;

public class RabbitMqPublisher(IConnection connection) : IEventPublisher
{
    public async Task PublishAsync(byte[] body, string queueName)
    {
        var channel = await connection.CreateChannelAsync();
        
        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: false,
            autoDelete: false,
            arguments: null);

        
        await channel.BasicPublishAsync(
            exchange:string.Empty,
            routingKey:string.Empty,
            body: body);
    }
}