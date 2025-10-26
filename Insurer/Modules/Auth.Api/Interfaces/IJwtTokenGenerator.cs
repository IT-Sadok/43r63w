using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Interfaces
{
    public interface IJwtTokenGenerator
    {
        Result<string> GenerateToken(IdentityUser user);
    }
}