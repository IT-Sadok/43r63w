using Policy.Domain.Enums;

namespace Policy.Domain.Entities;

public class Document
{
    public int Id { get; set; }
    public int? PolicyId { get; set; }
    public int? PolicyClaimId { get; set; }
    public string FileName { get; set; } = null!;
    public FileType FileType { get; set; }
    public DateTime UploadedDate { get; set; }
    public Policy? Policy { get; set; }
    public PolicyClaim? PolicyClaim { get; set; }
}
