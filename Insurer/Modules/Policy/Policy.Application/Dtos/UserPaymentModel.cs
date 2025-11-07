

using Policy.Domain.Enums;

namespace Policy.Application.Dtos;

public sealed class UserPaymentModel
{
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.InProcess;
    public string? Notes { get; set; }
}