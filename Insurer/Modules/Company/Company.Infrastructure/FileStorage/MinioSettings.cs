namespace Company.Infrastructure.FileStorage;

public class MinioSettings
{
    public const string Key = "Minio";

    public string Endpoint { get; set; } = null!;

    public string AccessKey { get; set; } = null!;

    public string SecretKey { get; set; } = null!;

    public string Bucket { get; set; } = null!;

    public bool UseSsl { get; set; }

    public int FileSize { get; set; }
}