using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Policy.Infrastructure.Data;

namespace Policy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPolicyInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PolicyDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }
}
