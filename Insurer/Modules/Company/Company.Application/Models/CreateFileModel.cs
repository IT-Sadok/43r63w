namespace Company.Application.Models;

public sealed class CreateFileModel
{
    public string ObjectKey { get; set; } = null!;
    
    public byte[] Content { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? CompanyName { get; set; } = null!;

    public string? InsuranceType { get; set; }
}