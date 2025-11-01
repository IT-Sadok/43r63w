using Policy.Domain.Enums;

namespace Policy.Domain.Entities;

public class PolicyHistory
{
    public int Id { get; set; }
    public int PolicyId { get; set; }
    public ChangeType ChangeType { get; set; }
    public string ChangedBy { get; set; } = null!;
    public DateTime ChangeDate { get; set; }
    public string OldValue { get; set; } = null!;
    public string NewValue { get; set; } = null!;
    public string? Notes { get; set; }

    public Policy Policy { get; set; } = null!;
}
