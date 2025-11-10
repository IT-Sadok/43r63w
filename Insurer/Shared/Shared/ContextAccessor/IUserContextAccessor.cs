namespace Shared.ContextAccessor;

public interface IUserContextAccessor
{
    public UserContextModel GetUserContext();
}

public sealed class UserContextModel
{
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public IEnumerable<string> Roles { get; set; } = [];
}