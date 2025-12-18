using System.Text.Json;
using Notification.Api.Interfaces;
using Shared.Events;

namespace Notification.Api.EventsHandler;

public class PolicyUpdatedEventHandler(ILogger<PolicyUpdatedEventHandler> logger) : IEventHandler
{
    public string EventType => SC.PolicyUpdatedEvent;

    public Task HandleAsync(string message, CancellationToken cancellationToken)
    {
        var @event = JsonSerializer.Deserialize<PolicyUpdatedEvent>(message);

        logger.LogInformation($"Policy: {@event!.PolicyId} status was successfully updated");

        return Task.CompletedTask;
    }
}