using Company.Infrastructure.Data;
using Insurer.Host.Endpoints;
using Microsoft.EntityFrameworkCore;
using Policy.Infrastructure.Data;
using User.Infrastructure.Data;
namespace Insurer.Host.Configuration;
public static class MiddlewareExtension
{
    public static void SetupEndpoints(
        this IEndpointRouteBuilder app,
        IHostEnvironment env)
    {
        app.MapPolicyEndpoints();
        app.MapAuthEndpoints();
        app.MapCustomerEndpoints();
        app.MapAgentEndpoints();
        app.MapDocumentEndpoints();
        app.MapCompanyEndpoint(env);
        
        app.MapGet("/", () => Results.Redirect("/swagger"));
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

    public static async Task ApplyMigrationAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        var companyDb = scope.ServiceProvider.GetRequiredService<CompanyDbContext>();
        var companyPendingMigration = await companyDb.Database.GetPendingMigrationsAsync();

        if (companyPendingMigration.Any())
            await companyDb.Database.MigrateAsync();

        var policyDb = scope.ServiceProvider.GetRequiredService<PolicyDbContext>();
        var policyPendingMigration = await policyDb.Database.GetPendingMigrationsAsync();

        if (policyPendingMigration.Any())
            await policyDb.Database.MigrateAsync();

        var userDb = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        var userPendingMigration = await userDb.Database.GetPendingMigrationsAsync();

        if (userPendingMigration.Any())
            await userDb.Database.MigrateAsync();
    }
    
}