using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Application.Options;
using Auth.Domain.Domain;
using Shared.Results;

namespace Auth.Application.Services;

public class JwtTokenGenerator(IOptions<JwtOptions> options) : IJwtTokenGenerator
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public Result<GenerateTokenResponse> GenerateToken(
        ApplicationUser user,
        IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: creds,
            expires: DateTime.Now.AddMinutes(10),
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience
        );

        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return Result<GenerateTokenResponse>.Success(new GenerateTokenResponse { Token = token });
    }
}