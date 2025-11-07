namespace Policy.Application.Dtos;

public sealed class UserPaymentsModel
{
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
}