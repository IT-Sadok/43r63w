namespace Notification.Api.Interfaces;

public interface IEventHandler
{
    public string EventType { get; }

    Task HandleAsync(string message, CancellationToken cancellationToken);
}