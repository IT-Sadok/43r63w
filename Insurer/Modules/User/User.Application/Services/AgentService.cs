using Shared.Errors;
using Shared.Results;
using User.Application.Interfaces;
using User.Application.Mapper;
using User.Application.Models;
using User.Application.Models.Auth;
using User.Application.Models.Responses;
using User.Domain.Entity;
using User.Domain.Enums;
using User.Domain.ValueObject;
using User.Infrastructure.Data;

namespace User.Application.Services;

internal sealed class AgentService(
    IAuthService authServicePublic,
    UserDbContext userDbContext) : IAgentService
{
    public async Task<Result<CreateAgentResponse>> CreateAgentAsync(
        CreateAgentModel model,
        CancellationToken cancellationToken = default)
    {
        var entity = new Agent
        {
            UserId = model.UserId,
            CompanyId = model.CompanyId,
            FirstName = model.FirstName,
            MiddleName = model.MiddleName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            DateOfBirth = model.DateOfBirth,
            CommissionRate = 0,
            Address = new Address
            {
                Country = model.AddressModel.Country,
                City = model.AddressModel.City,
                Street = model.AddressModel.Street,
                ZipCode = model.AddressModel.ZipCode,
            },
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };

        userDbContext.Agents.Add(entity);
        await userDbContext.SaveChangesAsync(cancellationToken);

        var roleModel = new AssignRoleModel
        {
            UserId = entity.UserId.ToString(),
            Role = Role.Agent
        };

        await authServicePublic.AssignRolesAsync(roleModel, cancellationToken);

        return Result<CreateAgentResponse>.Success(new CreateAgentResponse { Success = true });
    }

    public async Task<Result<GetAgentModel>> GetAgentAsync(
        int Id,
        CancellationToken cancellationToken = default)
    {
        var agent = await userDbContext.Agents.FindAsync(Id, cancellationToken);
        return agent != null
            ? Result<GetAgentModel>.Success(agent.ToModel())
            : Result<GetAgentModel>.Failure(ErrorsMessage.EntityError);
    }
}

public sealed class GetAgentModel
{
    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string? Email { get; set; }

    public string PhoneNumber { get; set; } = null!;
}