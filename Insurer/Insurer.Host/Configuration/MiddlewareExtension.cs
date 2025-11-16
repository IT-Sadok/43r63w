using Insurer.Host.Endpoints;

namespace Insurer.Host.Configuration;

public static class MiddlewareExtension
{
    public static void SetupEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPolicyEndpoints();
        app.MapAuthEndpoints();
        app.MapCustomerEndpoints();
        app.MapAgentEndpoints();
    }
    
    public static void SetupSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}