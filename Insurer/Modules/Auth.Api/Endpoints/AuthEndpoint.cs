namespace Auth.Api.Endpoints;
public static class AuthEndpoint
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
        AuthService authService,
        RegisterModel request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(request, cancellationToken);
        return result.IsSuccess
            ? Results.Ok("Succeeded")
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> LoginAsync(
        AuthService authService,
        RegisterModel request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(request, cancellationToken);
        return result.IsSuccess
            ? Results.Ok("Succeeded")
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> MeAsync(
        AuthService authService,
        RegisterModel request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(request, cancellationToken);
        return result.IsSuccess
            ? Results.Ok("Succeeded")
            : Results.BadRequest(result.Errors);
    }

}
