using Auth.Application.Contracts;
using Auth.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Insurer.Host.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");

        group.MapPost("/register", RegisterAsync);
        group.MapPost("/login", LoginAsync);
        group.MapPost("/me", MeAsync)
            .RequireAuthorization();
    }

    private static async Task<IResult> RegisterAsync(
        [FromServices]IAuthServicePublic authService,
        RegisterModel request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(request, cancellationToken);
        return result.IsSuccess
            ? Results.Ok("Succeeded")
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> LoginAsync(
        [FromServices]IAuthServicePublic authService,
        LoginModel request,
        CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }
    private static async Task<IResult> MeAsync(
        [FromServices]IAuthServicePublic authService,
        CancellationToken cancellationToken)
    {
        var result = await authService.GetMeAsync(cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }
}