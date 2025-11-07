using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Policy.Domain.Entities;
using Policy.Domain.Enums;
using Shared.Pagination;
using Shared.Sorted;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Policy.Application.FilterExstension;

public static class PolicyExstension
{
    public static IQueryable<Domain.Entities.Policy> Filter(
        this IQueryable<Domain.Entities.Policy> query,
        PolicyFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.PolicyNumber))
            query = query.Where(e => e.PolicyNumber == filter.PolicyNumber);

        if (filter.PolicyType != null)
            query = query.Where(e => e.PolicyType == filter.PolicyType);

        if (filter.Status != null)
            query = query.Where(e => e.Status == filter.Status);

        if (filter.StartDate != null)
            query = query.Where(e => e.StartDate == filter.StartDate);


        if (filter.EndDate != null)
            query = query.Where(e => e.EndDate == filter.EndDate);

        if (filter.StartDate != null && filter.EndDate != null)
            query = query.Where(e => e.StartDate == filter.StartDate && e.EndDate == filter.EndDate);

        return query;
    }

    public static IQueryable<Domain.Entities.Policy> Paging(this IQueryable<Domain.Entities.Policy> query, PagingParams pagingParams)
    {
        var page = pagingParams.Page ?? 1;
        var pageSize = pagingParams.PageSize ?? 10;

        return query
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }


    public static IQueryable<Domain.Entities.Policy> Sort(
       this IQueryable<Domain.Entities.Policy> query,
       SortParams sortParams)
    {
        return sortParams.SortDirection == SortDirection.Descending
            ? query.OrderByDescending(KeySelector(sortParams.OrderBy))
            : query.OrderBy(KeySelector(sortParams.OrderBy));
    }

    private static Expression<Func<Domain.Entities.Policy, object>> KeySelector(string orderBy)
    {
        if (string.IsNullOrEmpty(orderBy))
            return s => s.StartDate;

        return orderBy switch
        {
            nameof(Policy.Domain.Entities.Policy.PolicyNumber) => s => s.PolicyNumber,
            nameof(Policy.Domain.Entities.Policy.PremiumAmount) => s => s.PremiumAmount,
            _ => s => s.StartDate,
        };

    }
}

public sealed class PolicyFilter : PaginationRequest
{
    public string? PolicyNumber { get; set; }

    public PolicyType? PolicyType { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public PolicyStatus? Status { get; set; }
}

public sealed class PagingParams
{
    public int? PageSize { get; set; }
    public int? Page { get; set; }
}