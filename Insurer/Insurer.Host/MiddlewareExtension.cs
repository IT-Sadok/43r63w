using Insurer.Host.Endpoints;

namespace Insurer.Host;

public static class MiddlewareExtension
{
    public static void SetupEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPolicyEndpoints();
        app.MapAuthEndpoints();
        app.MapCustomerEndpoints();
        app.MapAgentEndpoints();
    }
}