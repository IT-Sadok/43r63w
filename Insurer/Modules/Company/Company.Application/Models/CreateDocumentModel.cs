using Policy.Domain.Enums;
using Shared.Enums;

namespace Company.Application.Models;

public sealed class CreateDocumentModel
{
    public string Prefix { get; set; } = null!;
    public int CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public FileType Type { get; set; }
}