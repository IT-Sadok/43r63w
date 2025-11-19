namespace Company.Domain.Entity;

public class Company
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = null!;
    public long RegistrationNumber { get; set; }

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<int> AgentIds { get; set; } = [];

    public List<int> PolicyIds { get; set; } = [];
    
    public List<Document> Documents { get; set; } = [];
}