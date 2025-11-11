using Auth.Application.Dtos;
using Shared.Results;

namespace Auth.Application.Contracts;

internal sealed class AuthServicePublic(IAuthService authService) : IAuthServicePublic
{
    public Task<Result<bool>> RegisterAsync(RegisterModel registerDto, CancellationToken cancellationToken)
        => authService.RegisterAsync(registerDto, cancellationToken);

    public Task<Result<string>> LoginAsync(LoginModel loginDto, CancellationToken cancellationToken)
        => authService.LoginAsync(loginDto, cancellationToken);

    public Task<Result<UserModel>> GetMeAsync(CancellationToken cancellationToken)
        => authService.GetMeAsync();
}