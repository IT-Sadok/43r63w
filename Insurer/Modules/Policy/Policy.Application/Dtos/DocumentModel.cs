

using Policy.Domain.Enums;

namespace Policy.Application.Dtos;

public sealed class DocumentModel
{
    public string FileName { get; set; } = null!;
    public FileType FileType { get; set; }
    public DateTime UploadedDate { get; set; }
}
