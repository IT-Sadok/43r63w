
namespace Policy.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int PolicyId { get; set; }

    public int PolicyClaimId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public string? Description { get; set; }

    public Policy? Policy { get; set; }
    public PolicyClaim? PolicyClaim { get; set; }
}
