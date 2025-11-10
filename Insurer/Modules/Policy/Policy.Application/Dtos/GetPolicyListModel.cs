using Shared.Pagination;


namespace Policy.Application.Dtos;

public sealed class GetPolicyListModel : PaginationRequest
{
    public string PolicyNumber { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
