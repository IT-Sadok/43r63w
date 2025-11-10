namespace Policy.Application.Dtos;

public sealed class UserPaymentsModel
{
    public string? UserId { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
}