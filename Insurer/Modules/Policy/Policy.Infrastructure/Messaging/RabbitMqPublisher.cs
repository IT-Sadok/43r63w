using System.Text;
using System.Text.Json;
using Policy.Infrastructure.Interfaces;
using RabbitMQ.Client;
using Shared;

namespace Policy.Infrastructure.Messaging;

public class RabbitMqPublisher(IConnection connection) : IEventPublisher
{
    public async Task PublishAsync(string eventType, string content, string queueName, CancellationToken cancellationToken)
    {
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
    
        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            autoDelete: false,
            exclusive: false,
            arguments: null, 
            cancellationToken: cancellationToken);
        
        var body = Encoding.UTF8.GetBytes(content);
        
        var props = new BasicProperties
        {
            Headers = new Dictionary<string, object>
            {
                ["eventType"] = eventType,
            }!
        };
    
        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            mandatory: false,
            basicProperties: props,
            body: body, 
            cancellationToken: cancellationToken);
    }
}