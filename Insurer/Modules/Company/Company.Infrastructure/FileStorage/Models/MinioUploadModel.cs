namespace Company.Infrastructure.FileStorage.Models;

public sealed class MinioUploadModel
{
    public string ObjectKey { get; set; } = null!;
    
    public byte[] Content { get; set; } = null!;

    public string Type { get; set; } = null!;
}