using Auth.Application.Dtos;
using Shared.Results;

namespace Auth.Application.Contracts;

public interface IAuthServicePublic
{
    public Task<Result<bool>> RegisterAsync(
        RegisterModel registerDto, CancellationToken cancellationToken);

    public Task<Result<string>> LoginAsync(
        LoginModel loginDto, CancellationToken cancellationToken);

    public Task<Result<UserModel>> GetMeAsync(CancellationToken cancellationToken);

    public Task<Result<bool>> AssignRoleAsync(
        AssignRoleModel model, 
        CancellationToken cancellationToken);
        
}