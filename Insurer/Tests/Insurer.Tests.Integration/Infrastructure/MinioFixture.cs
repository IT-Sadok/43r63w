using Minio;
using Minio.DataModel.Args;
using Testcontainers.Minio;

namespace Insurer.Tests.Integration.Infrastructure;

public class MinioFixture : IAsyncLifetime
{
    private readonly MinioContainer _minioContainer = new MinioBuilder()
        .WithUsername("test")
        .WithPassword("test123!")
        .Build();

    public MinioClient MinioClient { get; private set; } = new MinioClient();

    public const string BucketName = "test";
    
    public string Host;

    public ushort Port;

    public async Task InitializeAsync()
    {
        await _minioContainer.StartAsync();

        Host = _minioContainer.Hostname;
        Port = _minioContainer.GetMappedPublicPort(9000);

        MinioClient.WithEndpoint("test")
            .WithEndpoint(Host, Port)
            .WithCredentials("test", "test123!")
            .WithSSL(false)
            .Build();

        var isBucketExists = await MinioClient.BucketExistsAsync(new BucketExistsArgs()
            .WithBucket(BucketName));

        if (!isBucketExists)
        {
            await MinioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(BucketName));
        }
    }

    public async Task DisposeAsync()
    {
        await _minioContainer.StopAsync();
        await _minioContainer.DisposeAsync();
    }
}