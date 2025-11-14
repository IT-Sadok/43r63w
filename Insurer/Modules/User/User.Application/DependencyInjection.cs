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
        services.AddScoped<CustomerService>();
        services.AddScoped<AgentService>();

        services.AddScoped<ICustomerServicePublic, CustomerServicePublic>();
        services.AddScoped<IAgentServicePublic, AgentServicePublic>();
        return services;
    }
}