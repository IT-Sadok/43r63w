using System.Security.Claims;
using Auth.Domain.Domain;
using Shared.Results;

namespace Auth.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        Result<string> GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}