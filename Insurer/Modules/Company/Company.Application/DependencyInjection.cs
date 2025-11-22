using Company.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCompanyApplication(
        this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<DocumentService>();
        services.AddScoped<CompanyService>();
        return services;
    }
}