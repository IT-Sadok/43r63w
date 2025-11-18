using Shared.Enums;
namespace Company.Domain.Entity;

public class Document
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string ObjectName { get; set; } = null!;

    public string? Name { get; set; }

    public FileType Type { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }

    public Company Company  { get; set; }
}