using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Policy.Application.BackgroundService;
using Policy.Application.Options;
using Policy.Application.Services;
using Policy.Application.Validators;
using Policy.Infrastructure.Messaging;


namespace Policy.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddPolicyApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IPolicyService,PolicyService>();
        services.AddValidatorsFromAssembly(typeof(CreatePolicyValidator).Assembly);
        
        services.Configure<RabbitMqQueue>(configuration.GetSection("RabbitMqQueues"));
        services.Configure<OutboxProcessorOptions>(configuration.GetSection("OutboxProcessorOptions"));
        
        services.AddHostedService<EventDispatcher>();
        
        return services;
    }
}