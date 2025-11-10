using Microsoft.Extensions.DependencyInjection;
using Policy.Application.Contracts;
using Policy.Application.Services;


namespace Policy.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddPolicyApplication(
        this IServiceCollection services)
    {
        services.AddScoped<PolicyService>();
        services.AddScoped<IPolicyServicePublic, PolicyServicePublic>();
        return services;
    }
}
