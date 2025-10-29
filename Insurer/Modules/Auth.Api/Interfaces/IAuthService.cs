using Shared.Results;

namespace Auth.Api.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserModel>> GetMeAsync();
        Task<Result<string>> LoginAsync(LoginModel loginDto, CancellationToken cancellationToken);
        Task<Result<bool>> RegisterAsync(RegisterModel registerDto, CancellationToken cancellationToken);
    }
}