using Company.Application;
using Company.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Bootstrapper;

public static class CompanyModuleInstaller
{
    public static IServiceCollection AddCompanyModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCompanyApplication();
        services.AddCompanyInfrastructure(configuration);
        return services;
    } 
}