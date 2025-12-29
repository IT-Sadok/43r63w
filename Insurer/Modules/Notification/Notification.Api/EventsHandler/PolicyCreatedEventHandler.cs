using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Notification.Api.Data;
using Notification.Api.Interfaces;
using Shared.Events;

namespace Notification.Api.EventsHandler;

public sealed class PolicyCreatedEventHandler(
    ILogger<PolicyCreatedEventHandler> logger,
    NotificationDbContext dbContext) : IEventHandler
{
    public string EventType => SC.PolicyCreatedEvent;

    public async Task HandleAsync(string message, CancellationToken cancellationToken)
    {
        var @event = JsonSerializer.Deserialize<PolicyCreatedEvent>(message);

        if (@event == null)
            throw new ArgumentNullException("Event is null");
        
        logger.LogInformation($"Policy successfuly created with number of : {@event!.PolicyNumber}");
    }
}