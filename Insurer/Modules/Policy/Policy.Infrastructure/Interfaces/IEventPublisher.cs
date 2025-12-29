using Shared;

namespace Policy.Infrastructure.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync(
        string eventType, 
        string content, 
        CancellationToken cancellationToken);
}