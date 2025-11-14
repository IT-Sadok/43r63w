using User.Domain.ValueObject;

namespace User.Domain.Entity;

public class Agent
{
    public int Id { get; set; }
    
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public decimal CommissionRate { get; set; }

    public int CompanyId { get; set; }

    public Address Address { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}