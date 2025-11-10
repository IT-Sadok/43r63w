using System.Security.Claims;
using Shared.ContextAccessor;

namespace Insurer.Host.Utils;

public class UserContextAccessor(IHttpContextAccessor httpContextAccessor) : IUserContextAccessor
{
    public UserContextModel GetUserContext()
    {
        return new UserContextModel
        {
            UserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
            UserName = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name),
            Roles = httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role).Select(c => c.Value) ?? []
        };
    }
}