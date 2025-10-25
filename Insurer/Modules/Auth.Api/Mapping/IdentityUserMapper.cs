using Auth.Api.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Mapping;

public static class IdentityUserMapper
{
    public static IdentityUser ToEntity(this RegisterDto registerDto)
    {
        return new IdentityUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
        };
    }

    
    public static UserDto ToDto(this IdentityUser user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber!,
        };
    }
}

