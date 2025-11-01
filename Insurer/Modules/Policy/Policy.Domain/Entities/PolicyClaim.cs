using Policy.Domain.Enums;

namespace Policy.Domain.Entities;

public class PolicyClaim
{
    public int Id { get; set; }

    public int PolicyId { get; set; }

    public string ClaimNumber { get; set; } = null!;

    public decimal ClaimAmount { get; set; }

    public ClaimStatus Status { get; set; } = ClaimStatus.Submitted;

    public string? Description { get; set; }

    public DateTime ClaimDate { get; set; }

    public DateTime? DecisionDate { get; set; }

    public List<Document> Documents { get; set; } = [];
}
