using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using User.Application.Contracts;
using User.Application.Services;

namespace User.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddUserApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<Services.CustomerService>();
        services.AddScoped<Services.AgentService>();

        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAgentService, AgentService>();
        return services;
    }
}