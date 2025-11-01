
using Policy.Domain.Enums;

namespace Policy.Domain.Entities;


public class Policy
{
    public int Id { get; set; }

    public string PolicyNumber { get; set; } = null!;

    public int CustomerId { get; set; }

    public int CompanyId { get; set; }

    public int AgentId { get; set; }

    public PolicyType PolicyType { get; set; }

    public decimal CoverageAmount { get; set; }

    public decimal PremiumAmount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public PolicyStatus Status { get; set; } = PolicyStatus.Active;

    public List<PolicyClaim> PolicyClaims { get; set; } = [];

    public List<Document> Documents { get; set; } = [];

    public List<PolicyHistory> PolicyHistories { get; set; } = [];

    public List<Payment> Payments { get; set; } = [];
}
