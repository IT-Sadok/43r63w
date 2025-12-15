namespace Shared;

public abstract class BaseEvent
{
    public Guid EventId { get; } = Guid.NewGuid();

    public DateTime OccuredAt { get;} = DateTime.Now;
};