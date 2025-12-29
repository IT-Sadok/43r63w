using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Policy.Infrastructure.Interfaces;
using RabbitMQ.Client;
using Shared;

namespace Policy.Infrastructure.Messaging;

public class RabbitMqPublisher(
    IConnection connection,
    EventRouting eventRouting,
    IOptions<RabbitMqQueue> rabbitMqQueues) : IEventPublisher
{
    private readonly RabbitMqQueue _rabbitMqQueue = rabbitMqQueues.Value;

    public async Task PublishAsync(string eventType, string content, CancellationToken cancellationToken)
    {
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
        
        var arguments = new Dictionary<string, object?>
        {
            ["x-dead-letter-exchange"] = "",
            ["x-dead-letter-routing-key"] = "policy.created.dlq"
        };

        await channel.QueueDeclareAsync(
            queue: _rabbitMqQueue.PolicyCreated,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: arguments,
            cancellationToken: cancellationToken);

        var queueName = eventRouting.GetQueueName(eventType);

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