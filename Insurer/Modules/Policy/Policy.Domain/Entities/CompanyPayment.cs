using Policy.Domain.Enums;

namespace Policy.Domain.Entities;

public class CompanyPayment
{
    public int Id { get; set; }
    public int PolicyId { get; set; }
    public int ClaimId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.InProcess;
    public string? Notes { get; set; }
    public PolicyClaim Claim { get; set; } = null!;   
    public Policy Policy { get; set; } = null!;
}

