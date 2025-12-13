using System.ComponentModel.DataAnnotations;
using User.Domain.Enums;

namespace User.Application.Models.Auth;

public sealed class RegisterModel
{
    [Required]
    public string UserName { get; set; } = null!;
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string PhoneNumber { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;

    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;
    
    public Role? Role { get; set; }
}




