using Shared.Results;
using User.Application.Models;
using User.Application.Models.Responses;
using User.Application.Services;

namespace User.Application.Interfaces;

public interface IAgentService
{
    Task<Result<CreateAgentResponse>> CreateAgentAsync(CreateAgentModel model, CancellationToken cancellationToken = default);

    Task<Result<GetAgentModel>> GetAgentAsync(int Id, CancellationToken cancellationToken = default);
}
