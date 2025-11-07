


using Policy.Domain.Enums;

namespace Policy.Application.Dtos;

public sealed class PolicyClaimModel
{
    public string ClaimNumber { get; set; } = null!;

    public decimal ClaimAmount { get; set; }

    public ClaimStatus Status { get; set; } = ClaimStatus.Submitted;

    public string? Description { get; set; }

    public DateTime ClaimDate { get; set; }

    public DateTime? DecisionDate { get; set; }
}

