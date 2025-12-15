using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.ContextAccessor;
using User.Application.Interfaces;
using User.Application.Models;
using User.Domain.Enums;

namespace Insurer.Host.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("/customers")
            .RequireAuthorization();
        
        app.MapPost("", CreateCustomerAsync)
            .RequireAuthorization(new AuthorizeAttribute { Roles = nameof(Role.Agent) });
            
        app.MapGet("/{id}", GetCustomerAsync);
        app.MapPut("/{id}", UpdateCustomerAsync);
    }

    private static IResult UpdateCustomerAsync(
        [FromBody]UpdateCustomerModel model,
        [FromServices] ICustomerService customerService,
        CancellationToken cancellationToken = default)
    {
        return Results.Ok();
    }

    private static async Task<IResult> CreateCustomerAsync(
        [FromBody]CreateCustomerModel model,
        [FromServices] ICustomerService customerService,
        CancellationToken cancellationToken = default)
    {
        var result = await customerService.CreateCustomerAsync(model, cancellationToken);
        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> GetCustomerAsync(
        [FromRoute]int id,
        [FromServices] ICustomerService customerService,
        CancellationToken cancellationToken = default)
    {
        var result = await customerService.GetCustomerAsync(id, cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }
}