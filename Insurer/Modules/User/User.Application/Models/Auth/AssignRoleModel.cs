using User.Domain.Enums;

namespace User.Application.Models.Auth;

public sealed class AssignRoleModel
{
    public string UserId { get; set; } = null!;
    
    public Role Role { get; set; }
}