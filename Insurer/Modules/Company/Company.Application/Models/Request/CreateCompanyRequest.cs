namespace Company.Application.Models.Request;

public sealed class CreateCompanyRequest
{
    public string CompanyName { get; set; } = null!;
    
    public long RegistrationNumber { get; set; }

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;
}