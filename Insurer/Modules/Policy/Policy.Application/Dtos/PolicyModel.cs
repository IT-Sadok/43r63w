using Policy.Domain.Enums;

namespace Policy.Application.Dtos;

public sealed class PolicyModel
{
    public string PolicyNumber { get; set; } = null!;
    public PolicyType PolicyType { get; set; }

    public decimal CoverageAmount { get; set; }

    public decimal PremiumAmount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public PolicyStatus Status { get; set; } 
}
