namespace User.Application.Models.Auth;

public sealed class UserModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string? Email { get; set; }
    public string PhoneNumber { get; set; } = null!;
}