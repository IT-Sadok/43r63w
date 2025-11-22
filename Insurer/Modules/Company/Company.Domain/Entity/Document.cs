using Shared.Enums;

namespace Company.Domain.Entity;

public class Document
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public int CompanyId { get; set; }

    public string ObjectName { get; set; } = null!;

    public string? Name { get; set; }

    public string Type { get; set; } = null;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Company Company { get; set; }
}