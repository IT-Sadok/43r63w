namespace Company.Application.Models.Responses;

public sealed class GetCompanyResponse
{
    public string CompanyName { get; set; } = null!;

    public string? Email { get; set; } 
    
    public string PhoneNumber { get; set; } = null!;
}