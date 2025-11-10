

namespace Shared.Pagination;

public sealed class PaginationResponse<T>
{
    public PaginationResponse(
        int pageSize,
        int page,
        int totalCount,
        int totalPages,
        List<T> items)
    {
        PageSize = pageSize;
        Page = page;
        TotalCount = totalCount;
        TotalPages = totalPages;
        Items = items;
    }

    public int PageSize { get; set; }

    public int Page { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public List<T> Items { get; set; }
}

