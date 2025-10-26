using System.ComponentModel.DataAnnotations;

namespace Auth.Api.Dtos;

public sealed class LoginDto
{
    public string UserName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Password { get; set; } = null!;
    [EmailAddress]
    public string? Email { get; set; }
}

