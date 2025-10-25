using Auth.Api.Data;
using Auth.Api.Dtos;
using Auth.Api.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Api.Services;

public class AuthService(
    UserManager<IdentityUser> userManager,
    IConfiguration configuration)
{
    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        var user = registerDto.ToEntity();
        var result = await userManager.CreateAsync(user, registerDto.Password);
        return result.Succeeded;
    }
    public async Task<string?> LoginAsync(LoginDto loginDto)
    {
        var user = await userManager.FindByNameAsync(loginDto.UserName);
        if (user is null)
            return null;

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result)
            return null;

        return await GenerateTokenAsync(user);
    }

    public async Task<string> GenerateTokenAsync(IdentityUser user)
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

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key")!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        var securityToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: creds,
            expires: DateTime.Now.AddMinutes(10),
            issuer: configuration.GetValue<string>("Jwt:Issuer"),
            audience: configuration.GetValue<string>("Jwt:Audience")
            );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
    public async Task<UserDto> GetMeAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
            throw new Exception("User not found");

        return user.ToDto();
    }

}


