using Microsoft.AspNetCore.Mvc;
using Policy.Application.Contracts;
using Policy.Application.Dtos;
using Policy.Application.FilterExstension;
using Shared.ContextAccessor;
using Shared.Results;
using Shared.Sorted;

namespace Insurer.Host.Endpoints;

public static class PolicyEndpoints
{
    public static void MapPolicyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/policy").RequireAuthorization();

        group.MapPost("/create", CreatePolicyAsync);
        group.MapGet("/get/{id:int}", GetPolicyAsync);
        group.MapGet("/get", GetPoliciesAsync);
        group.MapPut("/update", UpdatePolicyAsync);
        group.MapDelete("/delete/{id:int}", DeletePolicyAsync);
    }

    private static async Task<IResult> CreatePolicyAsync(
        CreatePolicyModel model,
        [FromServices] IPolicyServicePublic policyService,
        [FromServices] IUserContextAccessor userContext,
        CancellationToken cancellationToken = default)
    {
        var user = userContext.GetUserContext();
        model.UserPaymentsModel = new UserPaymentsModel
        {
            UserId = user.UserId,
            Amount = model.UserPaymentsModel.Amount,
            Notes = model.UserPaymentsModel.Notes
        };

        var result = await policyService.CreatePolicyAsync(model, cancellationToken);

        return result.IsSuccess
            ? Results.Ok("Succeeded")
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> GetPolicyAsync(
        [FromRoute] int id,
        [FromServices] IPolicyServicePublic policyService,
        CancellationToken cancellationToken = default)
    {
        var result = await policyService.GetPolicyAsync(new GetPolicyModel { Id = id }, cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(result.ErrorMessage);
    }

    private static async Task<IResult> GetPoliciesAsync(
        [AsParameters] PolicyFilter filter,
        [AsParameters] SortParams sortParams,
        [FromServices] IPolicyServicePublic policyService,
        CancellationToken cancellationToken = default)
    {
        var result = await policyService.GetPoliciesAsync(filter, sortParams, cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(result.ErrorMessage);
    }

    private static async Task<IResult> UpdatePolicyAsync(PolicyUpdateModel model,
        [FromServices] IPolicyServicePublic policyService,
        [FromServices] IUserContextAccessor userContext,
        CancellationToken cancellationToken = default)
    {
        var user = userContext.GetUserContext();
        model.UserName = user.UserName!;
        var result = await policyService.UpdatePolicyAsync(model, cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.ErrorMessage);
    }

    private static async Task<IResult> DeletePolicyAsync(
        [FromRoute] int id,
        [FromServices] IPolicyServicePublic policyService,
        [FromServices] IUserContextAccessor userContext,
        CancellationToken cancellationToken = default)
    {
        var result = await policyService.DeletePolicyAsync(id, cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest();
    }
}