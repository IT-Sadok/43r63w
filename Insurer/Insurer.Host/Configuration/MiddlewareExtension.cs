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
        app.MapDocumentEndpoints();
    }
    
    public static void SetupSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    public static void SetupExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
    }
}