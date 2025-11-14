using Microsoft.AspNetCore.Mvc;
using Shared.ContextAccessor;
using User.Application.Contracts;
using User.Application.Models;

namespace Insurer.Host.Endpoints;

public static class UserEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("customer").RequireAuthorization();
        
        app.MapPost("", CreateCustomerAsync);
        app.MapGet("{id}", GetCustomerAsync);
        app.MapPut("{id}", UpdateCustomerAsync);
    }

    private static IResult UpdateCustomerAsync(
        UpdateCustomerModel model,
        [FromServices] ICustomerServicePublic customerService,
        CancellationToken cancellationToken = default)
    {
        return Results.Ok();
    }

    public static async Task<IResult> CreateCustomerAsync(
        CreateCustomerModel model,
        [FromServices] ICustomerServicePublic customerService,
        CancellationToken cancellationToken = default)
    {
        var result = await customerService.CreateCustomerAsync(model, cancellationToken);
        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Errors);
    }

    public static async Task<IResult> GetCustomerAsync(
        int id,
        [FromServices] ICustomerServicePublic customerService,
        CancellationToken cancellationToken = default)
    {
        var result = await customerService.GetCustomerAsync(id, cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }
}