namespace User.Application.Models;

public sealed class CreateAgentModel
{
    public int UserId { get; set; }

    public int CompanyId { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string PhoneNumber { get; set; } = null!;
    
    public AddressModel AddressModel { get; set; } = null!;
    
    public DateTime DateOfBirth { get; set; }
}