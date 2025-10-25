namespace Auth.Api.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserDto>> GetMeAsync();
        Task<Result<string>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
        Task<Result<bool>> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
    }
}