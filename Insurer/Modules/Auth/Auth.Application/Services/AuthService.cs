using Auth.Application.Dtos;
using Auth.Domain.Domain;
using Shared.ContextAccessor;
using Shared.Results;

namespace Auth.Application.Services;

internal sealed class AuthService(
    UserManager<ApplicationUser> userManager,
    IJwtTokenGenerator jwtTokenGenerator,
    IUserContextAccessor userContextAccessor) : IAuthService
{
    public async Task<Result<bool>> RegisterAsync(RegisterModel registerDto, CancellationToken cancellationToken)
    {
        var user = registerDto.ToEntity();
        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, nameof(registerDto.Role));
        }

        return !result.Succeeded
            ? Result<bool>.Failure("Something went wrong",
                errors: result.Errors.ToDictionary(k => k.Code, v => v.Description))
            : Result<bool>.Success(result.Succeeded);
    }

    public async Task<Result<bool>> AssignRolesAsync(AssignRoleModel model, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(model.UserId.ToString());
        if (user == null)
            return Result<bool>.Failure("User not found");

        var roles = await userManager.GetRolesAsync(user);
        if (roles.Contains(nameof(model.Role)))
            return Result<bool>.Failure("User is already assigned");

        var result = await userManager.AddToRoleAsync(user, nameof(model.Role));
        return Result<bool>.Success(true);
    }

    public async Task<Result<string>> LoginAsync(LoginModel loginDto, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(loginDto.UserName);
        if (user is null)
            return Result<string>.Failure($"User not found");

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result)
            return Result<string>.Failure($"Password did not match");

        var roles = await userManager.GetRolesAsync(user);
        var token = jwtTokenGenerator.GenerateToken(user, roles);
        return token.IsSuccess
            ? Result<string>.Success(token.Value!)
            : Result<string>.Failure("Something went wrong");
    }

    public async Task<Result<UserModel>> GetMeAsync()
    {
        var userId = userContextAccessor.GetUserContext();

        if (string.IsNullOrEmpty(userId.UserId))
            return Result<UserModel>.Failure("User not found");

        var user = await userManager.FindByIdAsync(userId.UserId);

        return user == null ? Result<UserModel>.Failure("User not found") : Result<UserModel>.Success(user.ToDto());
    }
}