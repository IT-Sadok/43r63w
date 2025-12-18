using Shared;

namespace Policy.Infrastructure.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, string queueName, CancellationToken cancellationToken);
}