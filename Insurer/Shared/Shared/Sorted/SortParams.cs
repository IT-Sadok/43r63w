
namespace Shared.Sorted;

public sealed class SortParams
{
    public string? SortBy { get; set; }
    public string? OrderBy { get; set; }
    public SortDirection? SortDirection { get; set; }
}


public enum SortDirection
{
    Ascending,
    Descending,
}