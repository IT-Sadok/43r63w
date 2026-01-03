using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Notification.Api.Data;
using Notification.Api.Interfaces;
using Notification.Api.Models;
using Shared.Events;

namespace Notification.Api.EventsHandler;

public sealed class PolicyCreatedEventHandler(
    ILogger<PolicyCreatedEventHandler> logger,
    IEmailSender emailService) : IEventHandler
{
    public string EventType => SC.PolicyCreatedEvent;

    public async Task HandleAsync(string message, CancellationToken cancellationToken)
    {
        var @event = JsonSerializer.Deserialize<PolicyCreatedEvent>(message);

        ArgumentNullException.ThrowIfNull(@event);

        var model = new SendEmailModel(@event.Email, "Policy has been succefully created", "Policy Created");

        var response = await emailService.SendEmailAsync(model, cancellationToken);

        logger.LogInformation($"Policy successfuly created with number of : {@event!.PolicyNumber}");
    }
}