using Insurer.Tests.Integration.Infrastructure;

namespace Insurer.Tests.Integration;

public class IntegrationTestFixture : IAsyncLifetime
{
    public MinioFixture Minio { get; } = new();
    public CompanyFactory Factory { get; } = new();

    public async Task InitializeAsync()
    {
        await Minio.InitializeAsync();
        Factory.MinioFixture = Minio;
        await Factory.InitializeAsync();
        _ = Factory.CreateClient(); 
    }

    public Task DisposeAsync()
    {
        return Minio.DisposeAsync();
    }
}