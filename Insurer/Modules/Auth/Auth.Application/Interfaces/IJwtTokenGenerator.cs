using System.Security.Claims;
using Auth.Domain.Domain;
using Shared.Results;

namespace Auth.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        Result<GenerateTokenResponse> GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}


public sealed class GenerateTokenResponse
{
    public string? Token { get; set; }
}