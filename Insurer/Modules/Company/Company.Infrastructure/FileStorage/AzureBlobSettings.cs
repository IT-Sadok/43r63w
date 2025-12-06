namespace Company.Infrastructure.FileStorage;

public class AzureBlobSettings
{
    public string ConnectionString { get; set; } = null!;

    public string Container { get; set; } = null!;
}