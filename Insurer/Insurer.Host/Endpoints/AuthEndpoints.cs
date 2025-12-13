using Microsoft.AspNetCore.Mvc;
using Shared.ContextAccessor;
using User.Application.Interfaces;
using User.Application.Models.Auth;

namespace Insurer.Host.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");

        group.MapPost("/register", RegisterAsync);
        group.MapPost("/login", LoginAsync);

        group.MapPut("/assign-role", AssignRoleAsync)
            .RequireAuthorization();
        group.MapPost("/me", MeAsync)
            .RequireAuthorization();
    }

    private static async Task<IResult> RegisterAsync(
        [FromServices] IAuthService authService,
        RegisterModel request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(request, cancellationToken);
        return result.IsSuccess
            ? Results.Ok("Succeeded")
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> LoginAsync(
        [FromServices] IAuthService authService,
        [FromBody] LoginModel request,
        CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.ErrorMessage);
    }

    private static async Task<IResult> MeAsync(
        [FromServices] IAuthService authService,
        CancellationToken cancellationToken)
    {
        var result = await authService.GetMeAsync(cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> AssignRoleAsync(
        [FromServices] IAuthService authService,
        [FromServices] IUserContextAccessor userContextAccessor,
        AssignRoleModel model,
        CancellationToken cancellationToken)
    {
        var user = userContextAccessor.GetUserContext();
        model.UserId = user.UserId!;

        var result = await authService.AssignRolesAsync(model, cancellationToken);
        return result.IsSuccess
            ? Results.Ok("Succeeded")
            : Results.BadRequest(result.Errors);
    }
}