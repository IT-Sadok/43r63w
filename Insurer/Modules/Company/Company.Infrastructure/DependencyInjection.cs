using Company.Infrastructure.FileStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
namespace Company.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCompanyInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IFileStorageRepository,MinioFileStorageRepository>();
        services.Configure<MinioSettings>(configuration.GetSection("Minio"));
        services.AddSingleton<IMinioClient>(setup =>
        {
            var settings = setup.GetRequiredService<IOptions<MinioSettings>>().Value;

            return new MinioClient()
                .WithEndpoint(settings.Endpoint)
                .WithCredentials(settings.AccessKey, settings.SecretKey)
                .WithSSL(settings.UseSsl)
                .Build();
        });
        return services;
    }
}