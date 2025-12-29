namespace Policy.Domain.Entities;

public class OutboxMessage
{
    public int Id { get; set; }
    public required string Type { get; set; }
    
    public required string Content { get; set; }
    
    public DateTime OccurredOn { get; set; }
    
    public DateTime? ProcessedOn { get; set; }

    public string? Error { get; set; }
}