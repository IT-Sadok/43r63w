using Policy.Application.Dtos;
using Policy.Application.FilterExstension;
using Policy.Application.Services;
using Shared.Pagination;
using Shared.Results;
using Shared.Sorted;

namespace Policy.Application.Contracts;

public interface IPolicyServicePublic
{
    public Task<Result<bool>> CreatePolicyAsync(
        CreatePolicyModel model,
        CancellationToken cancellationToken = default);

    public Task<Result<bool>> UpdatePolicyAsync(
       PolicyUpdateModel model,
       CancellationToken cancellationToken = default);

    public Task<Result<PolicyModel>> GetPolicyAsync(
     GetPolicyModel model,
     CancellationToken cancellationToken = default);

    public Task<Result<PaginationResponse<PolicyModel>>> GetPoliciesAsync(
       PolicyFilter request,
       SortParams sortParams,
       PagingParams paginationParams,
       CancellationToken cancellationToken = default);

    public Task GetDetailAsync();
}

internal sealed class PolicyServicePublic(PolicyService policyService) : IPolicyServicePublic
{
    public Task<Result<bool>> CreatePolicyAsync(CreatePolicyModel model, CancellationToken cancellationToken = default)
     => policyService.CreatePolicyAsync(model, cancellationToken);

    public Task GetDetailAsync()
        => policyService.GetDetailAsync();


    public Task<Result<PaginationResponse<PolicyModel>>> GetPoliciesAsync(
        PolicyFilter request,
        SortParams sortParams,
        PagingParams paginationParams,
        CancellationToken cancellationToken = default)
     => policyService.GetPoliciesAsync(request, sortParams, paginationParams, cancellationToken);

    public Task<Result<PolicyModel>> GetPolicyAsync(GetPolicyModel model, CancellationToken cancellationToken = default)
     => policyService.GetPolicyAsync(model, cancellationToken);

    public Task<Result<bool>> UpdatePolicyAsync(PolicyUpdateModel model, CancellationToken cancellationToken = default)
     => policyService.UpdatePolicyAsync(model, cancellationToken);


}