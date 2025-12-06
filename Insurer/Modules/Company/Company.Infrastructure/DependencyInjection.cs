using Azure.Storage.Blobs;
using Company.Infrastructure.Data;
using Company.Infrastructure.FileStorage;
using Company.Infrastructure.FileStorage.AzureBlobStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using Microsoft.Extensions.Hosting;

namespace Company.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCompanyInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment env)
    {
        services.AddScoped<IFileStorageRepository, MinioFileStorageRepository>();
        services.Configure<MinioSettings>(configuration.GetSection("Minio"));

        if (env.IsDevelopment())
        {
            var minioSettings = configuration.GetSection("Minio");
            services.Configure<MinioSettings>(minioSettings);

            services.AddSingleton<IMinioClient>(setup =>
            {
                var settings = setup.GetRequiredService<IOptions<MinioSettings>>().Value;
                if (string.IsNullOrWhiteSpace(settings.Endpoint))
                    return null!;

                return new MinioClient()
                    .WithEndpoint(settings.Endpoint)
                    .WithCredentials(settings.AccessKey, settings.SecretKey)
                    .WithSSL(settings.UseSsl)
                    .Build();
            });
        }
        else
        {
            var azureSettings = configuration.GetSection("AzureBlob");
            services.Configure<AzureBlobSettings>(azureSettings);

            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<AzureBlobSettings>>().Value;
                return new BlobServiceClient(settings.ConnectionString);
            });

            services.AddScoped<IFileStorageRepository, AzureBlobFileStorageRepository>();
        }


        services.AddDbContext<CompanyDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        return services;
    }
}