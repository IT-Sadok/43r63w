using Microsoft.Extensions.DependencyInjection;

namespace Company.Bootstrapper;

public static class CompanyModuleInstaller
{
    public static IServiceCollection AddCompanyModule(this IServiceCollection services)
    {
        return services;
    } 
}