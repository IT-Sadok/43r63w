using Auth.Application.Dtos;
using Auth.Domain.Domain;

namespace Auth.Application.Mapping;

public static class ApplicationUserMapper
{
    public static ApplicationUser ToEntity(this RegisterModel registerDto)
    {
        return new ApplicationUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
        };
    }
    public static UserModel ToDto(this ApplicationUser user)
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

