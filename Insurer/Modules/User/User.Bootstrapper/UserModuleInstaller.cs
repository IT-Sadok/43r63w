using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.Application;
using User.Infrastructure;

namespace User.Bootstrapper;

public static class UserModuleInstaller
{
    public static IServiceCollection AddUserModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddUserApplication(configuration);
        services.AddUserInfrastructure(configuration);
        return services;
    }
}