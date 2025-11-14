using Auth.Domain.Enums;

namespace Auth.Application.Dtos;

public sealed class AssignRoleModel
{
    public int UserId { get; set; }
    
    public Role Role { get; set; }
}