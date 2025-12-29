using Microsoft.Extensions.Options;
using Policy.Infrastructure.Data;
using Policy.Infrastructure.Interfaces;
using Policy.Infrastructure.Messaging;
using Policy.Infrastructure.Options;
using RabbitMQ.Client;


namespace Policy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPolicyInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PolicyDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
        services.Configure<EventRoutingOptions>(routingOptions =>
        {
            routingOptions.Routes = configuration
                .GetSection("EventRouting")
                .Get<Dictionary<string, string>>()!;
        });

        services.AddScoped<EventRouting>();

        services.AddSingleton<IConnection>(sp =>
        {
            var rabbitMqOptions = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqOptions.Host,
                Port = rabbitMqOptions.Port,
                VirtualHost = rabbitMqOptions.VirtualHost,
                UserName = rabbitMqOptions.UserName,
                Password = rabbitMqOptions.Password,
            };

            return factory.CreateConnectionAsync()
                .GetAwaiter()
                .GetResult();
        });

        services.AddScoped<IEventPublisher, RabbitMqPublisher>();

        return services;
    }
}