using System.Security.Claims;

namespace Auth.Api.Utils;

public static class ClaimsExstensions
{
    public static Guid GetUserId(this ClaimsPrincipal claims)
    {
        var userId = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userId, out var guid) 
            ? guid 
            : Guid.Empty;
    }

}

