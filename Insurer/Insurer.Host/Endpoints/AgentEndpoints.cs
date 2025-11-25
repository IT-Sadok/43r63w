using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;
using User.Application.Contracts;
using User.Application.Models;

namespace Insurer.Host.Endpoints;

public static class AgentEndpoints
{
    public static void MapAgentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/agents").RequireAuthorization();

        group.MapPost("", CreateAgentAsync);
        group.MapGet("{id}", CreateAgentAsync);
    }

    private static async Task<IResult> CreateAgentAsync(
        [FromBody] CreateAgentModel model,
        [FromServices] IAgentService agentService,
        CancellationToken cancellationToken)
    {
        var result = await agentService.CreateAgentAsync(model, cancellationToken);
        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest();
    }

    private static async Task<IResult> GetAgentAsync(
        [FromRoute]int id,
        [FromServices] IAgentService agentService,
        CancellationToken cancellationToken)
    {
        var result = await agentService.GetAgentAsync(id, cancellationToken);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.ErrorMessage);
    }
}