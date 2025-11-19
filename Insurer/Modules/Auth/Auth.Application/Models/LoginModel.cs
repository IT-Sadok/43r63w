using System.ComponentModel.DataAnnotations;

namespace Auth.Application.Dtos;

public sealed class LoginModel
{
    public string UserName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Password { get; set; } = null!;
    [EmailAddress]
    public string? Email { get; set; }
}

