namespace Company.Application.Models.Request;

public sealed class UpdateCompanyRequest
{
    public int Id { get; set; }
    
    public string? CompanyName { get; set; }
    
    public string? Email { get; set; }
    
    public string? Phone { get; set; }
}