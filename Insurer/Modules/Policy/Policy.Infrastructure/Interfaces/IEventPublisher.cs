using Shared;

namespace Policy.Infrastructure.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync(BaseEvent @event, string queueName);
}