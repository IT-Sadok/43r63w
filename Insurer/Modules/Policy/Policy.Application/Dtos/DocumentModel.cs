

using Policy.Domain.Enums;
using Shared.Enums;

namespace Policy.Application.Dtos;

public sealed class DocumentModel
{
    public string FileName { get; set; } = null!;
    public FileType FileType { get; set; }
    public DateTime UploadedDate { get; set; }
}
