using Auth.Application;
using Auth.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Bootstrapper;

public static class AuthModuleInstaller
{
    public static IServiceCollection AddAuthModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthInfrastructure(configuration);
        services.AddAuthApplication(configuration);
        return services;
    }
}