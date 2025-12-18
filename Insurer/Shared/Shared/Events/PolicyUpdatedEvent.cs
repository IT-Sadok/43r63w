namespace Shared.Events;

public class PolicyUpdatedEvent : BaseEvent
{
    public string UserId { get; set; } = null!;
    
    public string PolicyId { get; set; } = null!;

    public string Status { get; set; } = null!;
}