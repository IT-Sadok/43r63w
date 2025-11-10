using Policy.Domain.Enums;

namespace Policy.Application.Dtos;

public sealed class CreatePolicyModel
{
    public string PolicyNumber { get; set; } = null!;
    public decimal CoverageAmount { get; set; }

    public decimal PremiumAmount { get; set; }

    public PolicyType PolicyType { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public UserPaymentsModel UserPaymentsModel { get; set; }
}