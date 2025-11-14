using Shared.Results;
using User.Application.Models;
using User.Application.Services;

namespace User.Application.Contracts;

public interface IAgentServicePublic
{
    Task<Result<bool>> CreateAgentAsync(CreateAgentModel model, CancellationToken cancellationToken = default);

    Task<Result<GetAgentModel>> GetAgentAsync(int Id, CancellationToken cancellationToken = default);
}

internal sealed class AgentServicePublic(AgentService agentService) : IAgentServicePublic
{
    public Task<Result<bool>> CreateAgentAsync(
        CreateAgentModel model,
        CancellationToken cancellationToken = default)
        => agentService.CreateAgentAsync(model, cancellationToken);

    public Task<Result<GetAgentModel>> GetAgentAsync(
        int Id,
        CancellationToken cancellationToken = default)
        => agentService.GetAgentAsync(Id, cancellationToken);
}