using Auth.Application.Dtos;
using Auth.Application.Models.Responses;
using Shared.Results;

namespace Auth.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserModel>> GetMeAsync(CancellationToken cancellationToken);
        Task<Result<LoginResponse>> LoginAsync(LoginModel loginDto, CancellationToken cancellationToken);
        Task<Result<RegisterResponse>> RegisterAsync(RegisterModel registerDto, CancellationToken cancellationToken);
        Task<Result<AssignRoleResponse>> AssignRolesAsync(AssignRoleModel model, CancellationToken cancellationToken);
    }
}