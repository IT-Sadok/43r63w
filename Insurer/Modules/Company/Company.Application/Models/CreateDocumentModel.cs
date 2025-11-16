using Policy.Domain.Enums;
using Shared.Enums;

namespace Company.Application.Models;

public sealed class CreateDocumentModel
{
    public int CompanyId { get; set; }

    public string FileStorageId { get; set; } = null!;

    public string? Name { get; set; }

    public FileType Type { get; set; }
}