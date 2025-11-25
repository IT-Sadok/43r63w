using Policy.Application.Dtos;
using Policy.Application.Dtos.Responses;
using Policy.Application.FilterExstension;
using Shared.Pagination;
using Shared.Results;
using Shared.Sorted;

namespace Policy.Application.Services;

public interface IPolicyService
{
    Task<Result<PaginationResponse<PolicyModel>>> GetPoliciesAsync(
        PolicyFilter request,
        SortParams sortParams,
        CancellationToken cancellationToken = default);

    Task<Result<PolicyModel>> GetPolicyAsync(
        GetPolicyModel model,
        CancellationToken cancellationToken = default);

    Task<Result<CreatePolicyResponse>> CreatePolicyAsync(
        CreatePolicyModel model,
        CancellationToken cancellationToken = default);

    Task<Result<UpdatePolicyResponse>> UpdatePolicyAsync(
        PolicyUpdateModel model,
        CancellationToken cancellationToken = default);

    Task<Result<DeletePolicyResponse>> DeletePolicyAsync(int policyId, CancellationToken cancellationToken);
}