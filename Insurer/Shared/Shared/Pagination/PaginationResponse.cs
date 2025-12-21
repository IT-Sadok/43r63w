

namespace Shared.Pagination;

public sealed class PaginationResponse<T>(
    int pageSize,
    int page,
    int totalCount,
    int totalPages,
    List<T> items)
{
    public int PageSize { get; set; } = pageSize;

    public int Page { get; set; } = page;

    public int TotalCount { get; set; } = totalCount;

    public int TotalPages { get; set; } = totalPages;

    public List<T> Items { get; set; } = items;
}

