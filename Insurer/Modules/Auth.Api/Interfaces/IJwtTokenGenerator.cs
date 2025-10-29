using Microsoft.AspNetCore.Identity;
using Shared.Results;

namespace Auth.Api.Interfaces
{
    public interface IJwtTokenGenerator
    {
        Result<string> GenerateToken(IdentityUser user);
    }
}