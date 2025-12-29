namespace Notification.Api.Entities;

public class ProcessedEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid EventId { get; set; }

    public DateTime ProcessedAt { get; set; }
}