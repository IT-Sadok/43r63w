using Auth.Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Api.Services;

public class JwtTokenGenerator(IOptions<JwtOptions> options) : IJwtTokenGenerator
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public Result<string> GenerateToken(IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id),
            new Claim(ClaimTypes.Name,user.UserName!),
            new Claim(ClaimTypes.Role,"Admin"),
            user.Email is null
              ? new Claim(ClaimTypes.MobilePhone,user.PhoneNumber!)
              : new Claim(ClaimTypes.Email,user.Email),
        };

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

        return Result<string>.Success(token);
    }
}

