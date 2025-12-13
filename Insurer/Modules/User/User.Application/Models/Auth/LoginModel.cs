using System.ComponentModel.DataAnnotations;

namespace User.Application.Models.Auth;

public sealed class LoginModel
{
    public string UserName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Password { get; set; } = null!;
    [EmailAddress]
    public string? Email { get; set; }
}

