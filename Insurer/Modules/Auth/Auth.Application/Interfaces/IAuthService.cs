using Auth.Application.Dtos;
using Shared.Results;

namespace Auth.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserModel>> GetMeAsync();
        Task<Result<string>> LoginAsync(LoginModel loginDto, CancellationToken cancellationToken);
        Task<Result<bool>> RegisterAsync(RegisterModel registerDto, CancellationToken cancellationToken);
        Task<Result<bool>> AssignRolesAsync(AssignRoleModel model, CancellationToken cancellationToken);
    }
}