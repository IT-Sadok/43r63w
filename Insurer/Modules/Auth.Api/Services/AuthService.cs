using Auth.Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Results;

namespace Auth.Api.Services;

public class AuthService(
    UserManager<IdentityUser> userManager,
    JwtTokenGenerator jwtTokenGenerator,
    IUserContextAccessor userContextAccessor) : IAuthService
{
    public async Task<Result<bool>> RegisterAsync(RegisterModel registerDto, CancellationToken cancellationToken)
    {
        var user = registerDto.ToEntity();
        var result = await userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
            return Result<bool>.Failure("Something went wrong", errors: result.Errors.Select(e => e.Description));

        return Result<bool>.Success(result.Succeeded);
    }

    public async Task<Result<string>> LoginAsync(LoginModel loginDto, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(loginDto.UserName);
        if (user is null)
            return Result<string>.Failure($"User not found");

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result)
            return Result<string>.Failure($"Password didn`t match");

        var token = jwtTokenGenerator.GenerateToken(user);
        return token.IsSuccess == true
            ? Result<string>.Success(token.Value!)
            : Result<string>.Failure("Something went wrong");      
    }

    public async Task<Result<UserModel>> GetMeAsync()
    {
        var userId = userContextAccessor.GetUserContext();

        if (string.IsNullOrEmpty(userId.UserId))
            return Result<UserModel>.Failure("User not found");

        var user = await userManager.FindByIdAsync(userId.UserId);

        if (user == null)
            return Result<UserModel>.Failure("User not found");

        return Result<UserModel>.Success(user.ToDto());
    }

}


