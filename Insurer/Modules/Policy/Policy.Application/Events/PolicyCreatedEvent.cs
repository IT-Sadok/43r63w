using Shared;

namespace Policy.Application.Events;

public sealed class PolicyCreatedEvent : BaseEvent
{
    public string UserId { get; set; } = null!;

    public string PolicyId { get; set; } = null!;

    public string PolicyNumber { get; set; } = null!;

    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public decimal Price { get; set; }
}