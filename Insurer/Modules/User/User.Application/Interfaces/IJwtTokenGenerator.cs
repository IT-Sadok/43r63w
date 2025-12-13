using Shared.Results;
using User.Domain.Entity;

namespace User.Application.Interfaces;

public interface IJwtTokenGenerator
{
    Result<GenerateTokenResponse> GenerateToken(ApplicationUser user, IEnumerable<string> roles);
}


public sealed class GenerateTokenResponse
{
    public string? Token { get; set; }
}