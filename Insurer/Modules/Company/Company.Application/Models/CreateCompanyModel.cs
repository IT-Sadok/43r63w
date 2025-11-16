namespace Company.Application.Models;

public sealed class CreateCompanyModel
{
    public string CompanyName { get; set; } = null!;
    
    public long RegistrationNumber { get; set; }

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;
}