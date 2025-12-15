using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using User.Application;
using User.Infrastructure;

namespace User.Bootstrapper;

public static class UserModuleInstaller
{
    public static IServiceCollection AddUserModule(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddUserApplication(configuration, environment);
        services.AddUserInfrastructure(configuration);
        return services;
    }
}