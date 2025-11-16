using Auth.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.ContextAccessor;
using User.Application.Contracts;
using User.Application.Models;

namespace Insurer.Host.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("/customer")
            .RequireAuthorization();
        
        app.MapPost("create-customer", CreateCustomerAsync)
            .RequireAuthorization(new AuthorizeAttribute { Roles = nameof(Role.Agent) });
            
        app.MapGet("get-customer/{id}", GetCustomerAsync);
        app.MapPut("update-customer/{id}", UpdateCustomerAsync);
    }

    private static IResult UpdateCustomerAsync(
        [FromBody]UpdateCustomerModel model,
        [FromServices] ICustomerServicePublic customerService,
        CancellationToken cancellationToken = default)
    {
        return Results.Ok();
    }

    private static async Task<IResult> CreateCustomerAsync(
        [FromBody]CreateCustomerModel model,
        [FromServices] ICustomerServicePublic customerService,
        CancellationToken cancellationToken = default)
    {
        var result = await customerService.CreateCustomerAsync(model, cancellationToken);
        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> GetCustomerAsync(
        [FromRoute]int id,
        [FromServices] ICustomerServicePublic customerService,
        CancellationToken cancellationToken = default)
    {
        var result = await customerService.GetCustomerAsync(id, cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }
}