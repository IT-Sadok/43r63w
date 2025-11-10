using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Policy.Application.Contracts;
using Policy.Application.Services;
using Policy.Application.Validators;


namespace Policy.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddPolicyApplication(
        this IServiceCollection services)
    {
        services.AddScoped<PolicyService>();
        services.AddScoped<IPolicyServicePublic, PolicyServicePublic>();

        services.AddValidatorsFromAssembly(typeof(CreatePolicyValidator).Assembly);
        return services;
    }
}