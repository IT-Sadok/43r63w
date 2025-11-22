namespace Company.Application.Models.Request;

public sealed class DeleteFileRequest
{
    public string ObjectKey { get; set; } = null!;
}