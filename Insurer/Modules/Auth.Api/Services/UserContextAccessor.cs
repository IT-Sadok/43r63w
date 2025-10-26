using Auth.Api.Interfaces;
using System.Security.Claims;

namespace Auth.Api.Services;

public class UserContextAccessor : IUserContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UserContextModel GetUserContext()
    {
        return new UserContextModel
        {
            UserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier),
            UserName = _httpContextAccessor.HttpContext?.User?.Identity?.Name,
            Email = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email),
            Roles = _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role).Select(r => r.Value) ?? Enumerable.Empty<string>()
        };
    }
}

public sealed class UserContextModel
{
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public IEnumerable<string> Roles { get; set; } = [];
}