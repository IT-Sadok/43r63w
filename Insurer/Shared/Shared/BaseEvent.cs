namespace Shared;

public class BaseEvent
{
    public Guid EventId { get; set; } = Guid.NewGuid();

    public DateTime OccuredAt { get; set; } = DateTime.Now;

    public string EventType { get; set; } = null!;
};