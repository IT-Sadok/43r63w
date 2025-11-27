using System.Data.Common;
using System.Text;
using Auth.Application.Options;
using Company.Infrastructure.Data;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Insurer.Host;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Respawn;
using Testcontainers.MsSql;

namespace Insurer.Tests.Integration;

public class CompanyFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithPassword("MyStrongPassword123!")
        .Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;
    private HttpClient _httpClient;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d
                => d.ServiceType == typeof(DbContextOptions<CompanyDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            var connectionString = _container.GetConnectionString();
            services.AddDbContext<CompanyDbContext>(options => options.UseSqlServer(connectionString));
            
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