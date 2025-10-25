namespace Auth.Api.Interfaces;

public interface IUserContextAccessor
{
    string? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    IEnumerable<string> Roles { get; }
}
