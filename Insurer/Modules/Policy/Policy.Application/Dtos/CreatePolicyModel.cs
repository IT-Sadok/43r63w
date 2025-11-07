namespace Policy.Application.Dtos;

public sealed class CreatePolicyModel
{
    public decimal CoverageAmount { get; set; }

    public decimal PremiumAmount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public UserPaymentsModel UserPaymentsModel { get; set; }
}
