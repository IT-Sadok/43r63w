namespace Company.Application.Models.Responses;

public sealed class CreateCompanyResponse
{
    public int CompanyId { get; set; }
    public bool Success { get; set; }
}