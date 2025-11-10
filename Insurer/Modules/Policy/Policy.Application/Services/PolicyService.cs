using Microsoft.EntityFrameworkCore;
using Policy.Application.Dtos;
using Policy.Application.FilterExstension;
using Policy.Application.Mapping;
using Policy.Domain.Entities;
using Policy.Domain.Enums;
using Policy.Infrastructure.Data;
using Shared.Pagination;
using Shared.Results;
using Shared.Sorted;

namespace Policy.Application.Services;

internal sealed class PolicyService(
    PolicyDbContext policyDbContext)
{
    public async Task<Result<PaginationResponse<PolicyModel>>> GetPoliciesAsync(
        PolicyFilter request,
        SortParams sortParams,
        PagingParams paginationParams,
        CancellationToken cancellationToken = default)
    {
        var policies = await policyDbContext.Policies
            .Filter(request)
            .Sort(sortParams)
            .Paging(paginationParams)
            .Select(e => e.ToDto())
            .Select(e => e.ToModel())
            .ToListAsync(cancellationToken);

        var totalCount = await policyDbContext.Policies.CountAsync(cancellationToken);

        var totalPage = (int)Math.Round(totalCount / (double)(paginationParams.PageSize ?? 1));

        return Result<PaginationResponse<PolicyModel>>.Success(new PaginationResponse<PolicyModel>(
            pageSize: request.Page,
            page: request.PageSize,
            totalCount: totalCount,
            totalPages: totalPage,
            items: policies)
        );
    }

    public async Task<Result<PolicyModel>> GetPolicyAsync(
        GetPolicyModel model,
        CancellationToken cancellationToken = default)
    {
        var policy = await policyDbContext.Policies.FirstOrDefaultAsync(
            e => e.Id == model.PolicyId, cancellationToken);

        if (policy == null)
            return Result<PolicyModel>.Failure(ErrorsMessage.EntityError);

        var policyModel = policy.ToDto();

        if (policyModel == null)
            return Result<PolicyModel>.Failure("Can`t mapped policy");

        return Result<PolicyModel>.Success(policyModel!);
    }

    public async Task GetDetailAsync()
    {
        var response = await GetPolicyAsync(new GetPolicyModel
        {
            PolicyId = 123,
        });

        await Task.FromResult(response);
    }

    public async Task<Result<bool>> CreatePolicyAsync(
        CreatePolicyModel model,
        CancellationToken cancellationToken = default)
    {
        var validate = await createPolicyModelValidator.ValidateAsync(model, cancellationToken);
        if (!validate.IsValid)
            return Result<bool>.Failure(
                errorMessage: ErrorsMessage.ValidationError,
                errors: validate.Errors.ToDictionary(k => k.PropertyName, v => v.ErrorMessage));

        var policy = new Policy.Domain.Entities.Policy
        {
            PolicyNumber = model.PolicyNumber,
            AgentId = IdPlaceholder.AgentId,
            CompanyId = IdPlaceholder.CompanyId,
            CustomerId = IdPlaceholder.CustomerId,
            PolicyType = model.PolicyType,
            CoverageAmount = model.CoverageAmount,
            PremiumAmount = model.PremiumAmount,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Status = PolicyStatus.Pending,
            UserPayments =
            [
                new UserPayment
                {
                    Amount = model.UserPaymentsModel.Amount,
                    PaymentDate = DateTime.Now,
                    Status = PaymentStatus.Accepted,
                    Notes = model.UserPaymentsModel.Notes,
                }
            ]
        };

        policy.Status = PolicyStatus.Active;

        policyDbContext.Add(policy);
        await policyDbContext.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> UpdatePolicyAsync(
       PolicyUpdateModel model,
       CancellationToken cancellationToken = default)
    {
        var policyChange = new PolicyHistoryModel
        {
            ChangeType = ChangeType.PolicyUpdated,
            ChangedBy = "TestUser",
            ChangeDate = DateTime.Now,
            OldValue = model.PolicyHistoryModel.OldValue,
            NewValue = model.PolicyHistoryModel.NewValue,
            Notes = model.PolicyHistoryModel.Notes
        };

        policyDbContext.Add(policyChange);
        await policyDbContext.SaveChangesAsync(cancellationToken);

        var affected = await policyDbContext.Policies
            .Where(e => e.Id == model.PolicyId
            && e.Status != PolicyStatus.Cancelled
            && e.UserPayments.Any())
            .ExecuteUpdateAsync(up => up
            .SetProperty(p => p.Status, _ => model.PolicyStatus), cancellationToken);

        if (affected == 0)
            return Result<bool>.Failure("Something went wrong,try again");

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeletePolicyAsync(int policyId, CancellationToken cancellationToken)
    {
        var result = await policyDbContext.Policies.Where(e => e.Id == policyId).ExecuteDeleteAsync(cancellationToken);

        return result > 0
             ? Result<bool>.Success(true)
             : Result<bool>.Success(false);
    }
}
