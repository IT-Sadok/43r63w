using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Mapping;

public static class IdentityUserMapper
{
    public static IdentityUser ToEntity(this RegisterModel registerDto)
    {
        return new IdentityUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
        };
    }
    public static UserModel ToDto(this IdentityUser user)
    {
        return new UserModel
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber!,
        };
    }
}

