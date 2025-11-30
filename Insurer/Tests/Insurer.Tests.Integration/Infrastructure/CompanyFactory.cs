using System.Data.Common;
using System.Text;
using Company.Infrastructure.Data;
using Company.Infrastructure.FileStorage;
using Insurer.Host;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Minio;
using Respawn;
using Shared.ContextAccessor;
using Testcontainers.MsSql;

namespace Insurer.Tests.Integration.Infrastructure;

public class CompanyFactory :
    WebApplicationFactory<IApiMarker>,
    IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithPassword("MyStrongPassword123!")
        .Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;
    private HttpClient _httpClient;

    public MinioFixture MinioFixture;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            var minioSettings = new Dictionary<string, string>
            {
                ["MinioSettings:Endpoint"] = $"{MinioFixture.Host}:{MinioFixture.Port}",
                ["MinioSettings:AccessKey"] = "test",
                ["MinioSettings:SecretKey"] = "test123!",
                ["MinioSettings:Bucket"] = MinioFixture.BucketName
            };

            cfg.AddInMemoryCollection(minioSettings);
        });

        builder.ConfigureServices(services =>
        {
            var descriptorDbContext = services.SingleOrDefault(d
                => d.ServiceType == typeof(DbContextOptions<CompanyDbContext>));

            if (descriptorDbContext != null)
                services.Remove(descriptorDbContext);

            var userContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUserContextAccessor));

            if (userContextDescriptor != null)
                services.Remove(userContextDescriptor);
            
            var minioDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IMinioClient));

            if (minioDescriptor != null)
                services.Remove(minioDescriptor);

            var connectionString = _container.GetConnectionString();
            services.AddDbContext<CompanyDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IUserContextAccessor, TestUserContext>();
            
            services.AddSingleton<IMinioClient>(setup =>
            {
                var settings = setup.GetRequiredService<IOptions<MinioSettings>>();
                
                return new MinioClient()
                    .WithEndpoint($"{MinioFixture.Host}:{MinioFixture.Port}")
                    .WithCredentials("test", "test123!")
                    .WithSSL(false)
                    .Build();
            });

            services.PostConfigureAll<JwtBearerOptions>(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "test",
                    ValidAudience = "test",
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtGenerator.Key)),
                };
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        await InitRespawnerAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
        await _container.DisposeAsync();
    }

    public async Task ResetDbAsync() => await _respawner.ResetAsync(_dbConnection);

    public HttpClient CreateClient() => base.CreateClient();

    private async Task InitRespawnerAsync()
    {
        using (var scope = Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<CompanyDbContext>();
            await db.Database.MigrateAsync();
        }

        _dbConnection = new SqlConnection(_container.GetConnectionString());
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = ["company"],
        });
    }
}