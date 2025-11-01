using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Policy.Infrastructure;

namespace Policy.Bootstrapper;

public static class PolicyModule
{
    public static IServiceCollection AddPolicyModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPolicyInfrastructure(configuration);
        return services;
    }

}
