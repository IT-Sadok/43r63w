using System.Text.Json;
using Notification.Api.Interfaces;
using Shared.Events;

namespace Notification.Api.EventsHandler;

public sealed class PolicyCreatedEventHandler(ILogger<PolicyCreatedEventHandler> logger) : IEventHandler
{
    public string EventType => SC.PolicyCreatedEvent;

    public Task HandleAsync(string message, CancellationToken cancellationToken)
    {
        var @event = JsonSerializer.Deserialize<PolicyCreatedEvent>(message);

        logger.LogInformation($"Policy successfuly created with number of : {@event!.PolicyNumber}");

        return Task.CompletedTask;
    }
}