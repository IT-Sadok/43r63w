using Company.Application;
using Company.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Company.Bootstrapper;

public static class CompanyModuleInstaller
{
    public static IServiceCollection AddCompanyModule(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment env)
    {
        services.AddCompanyApplication();
        services.AddCompanyInfrastructure(configuration, env);
        return services;
    }
}