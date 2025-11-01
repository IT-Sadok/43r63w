namespace Policy.Domain.Entities;

public class Document
{
    public int Id { get; set; }
    public int? PolicyId { get; set; }
    public int? PolicyClaimId { get; set; }
    public string FileName { get; set; } = null!;
    public string FileType { get; set; } = null!;
    public DateTime UploadedDate { get; set; }
}
