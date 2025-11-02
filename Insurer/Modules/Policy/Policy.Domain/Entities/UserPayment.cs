using Policy.Domain.Enums;

namespace Policy.Domain.Entities;

public class UserPayment
{
    public int Id { get; set; }
    public int PolicyId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.InProcess;
    public string? Notes { get; set; }
    public Policy Policy { get; set; } = null!;
}
