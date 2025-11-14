using User.Application.Services;
using User.Domain.Entity;

namespace User.Application.Mapper;

public static class AgentMapper
{
    public static GetAgentModel ToModel(this Agent agent)
    {
        return new GetAgentModel
        {
            FirstName = agent.FirstName,
            LastName = agent.LastName,
            MiddleName = agent.MiddleName,
            Email = agent.Email,
            PhoneNumber = agent.PhoneNumber,
        };
    }
}