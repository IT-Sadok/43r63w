using Shared.Results;
using User.Application.Models;
using User.Application.Services;

namespace User.Application.Contracts;

public interface IAgentService
{
    Task<Result<bool>> CreateAgentAsync(CreateAgentModel model, CancellationToken cancellationToken = default);

    Task<Result<GetAgentModel>> GetAgentAsync(int Id, CancellationToken cancellationToken = default);
}
