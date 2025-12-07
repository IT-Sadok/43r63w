using Auth.Application.Dtos;
using Auth.Application.Models.Responses;
using Auth.Domain.Domain;
using Shared.ContextAccessor;
using Shared.Results;

namespace Auth.Application.Services;

internal sealed class AuthService(
    UserManager<ApplicationUser> userManager,
    IJwtTokenGenerator jwtTokenGenerator,
    IUserContextAccessor userContextAccessor,
    RoleManager<ApplicationRole> roleManager) : IAuthService
{
    public async Task<Result<RegisterResponse>> RegisterAsync(RegisterModel registerDto,
        CancellationToken cancellationToken)
    {
        var user = registerDto.ToEntity();
        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            var isRoleExists = await roleManager.RoleExistsAsync(registerDto.Role.ToString()!);
            if(!isRoleExists)
                await roleManager.CreateAsync(new ApplicationRole { Name = registerDto.Role.ToString() });
            
            await userManager.AddToRoleAsync(user, registerDto.Role.ToString()!);
        }

        return !result.Succeeded
            ? Result<RegisterResponse>.Failure("Something went wrong",
                errors: result.Errors.ToDictionary(k => k.Code, v => v.Description))
            : Result<RegisterResponse>.Success(new RegisterResponse
            {
                Success = result.Succeeded,
            });
    }

    public async Task<Result<AssignRoleResponse>> AssignRolesAsync(AssignRoleModel model,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(model.UserId);
        if (user == null)
            return Result<AssignRoleResponse>.Failure("User not found");

        var roles = await userManager.GetRolesAsync(user);
        if (roles.Contains(model.Role.ToString()))
            return Result<AssignRoleResponse>.Failure("User is already assigned");

        var result = await userManager.AddToRoleAsync(user, model.Role.ToString());
        return Result<AssignRoleResponse>.Success(new AssignRoleResponse
        {
            Success = result.Succeeded,
        });
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginModel loginDto, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(loginDto.UserName);
        if (user is null)
            return Result<LoginResponse>.Failure($"User not found");

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result)
            return Result<LoginResponse>.Failure($"Password did not match");

        var roles = await userManager.GetRolesAsync(user);
        var token = jwtTokenGenerator.GenerateToken(user, roles);
        return token.IsSuccess
            ? Result<LoginResponse>.Success(new LoginResponse { Token = token.Value!.Token })
            : Result<LoginResponse>.Failure("Something went wrong");
    }

    public async Task<Result<UserModel>> GetMeAsync(CancellationToken cancellationToken)
    {
        var userId = userContextAccessor.GetUserContext();

        if (string.IsNullOrEmpty(userId.UserId))
            return Result<UserModel>.Failure("User not found");

        var user = await userManager.FindByIdAsync(userId.UserId);

        return user == null ? Result<UserModel>.Failure("User not found") : Result<UserModel>.Success(user.ToDto());
    }
}