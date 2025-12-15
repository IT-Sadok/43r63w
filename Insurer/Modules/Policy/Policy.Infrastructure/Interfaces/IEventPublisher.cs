namespace Policy.Infrastructure.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync(byte[] body, string queueName);
}