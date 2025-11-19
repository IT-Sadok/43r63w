using Auth.Domain.Enums;

namespace Auth.Application.Dtos;

public sealed class AssignRoleModel
{
    public string UserId { get; set; } = null!;
    
    public Role Role { get; set; }
}