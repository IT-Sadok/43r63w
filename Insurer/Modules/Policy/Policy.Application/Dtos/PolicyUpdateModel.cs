using Policy.Domain.Enums;

namespace Policy.Application.Dtos;

public sealed class PolicyUpdateModel
{
    public string UserName { get; set; } = null!;
    public int PolicyId { get; set; }
    public PolicyStatus PolicyStatus { get; set; }

    public PolicyHistoryModel PolicyHistoryModel { get; set; }
}

public sealed class PolicyHistoryModel
{
    public ChangeType ChangeType { get; set; }
    public string ChangedBy { get; set; } = null!;
    public DateTime ChangeDate { get; set; }
    public string OldValue { get; set; } = null!;
    public string NewValue { get; set; } = null!;
    public string? Notes { get; set; }
}