using Insurer.Host.Utils;
using Shared.ContextAccessor;

namespace Insurer.Host;

public static class DependencyInjection
{
    public static IServiceCollection AddHostService(
        this IServiceCollection services)
    {
        services.AddScoped<IUserContextAccessor, UserContextAccessor>();
        services.AddHttpContextAccessor();
        return services;
    }
}