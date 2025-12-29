using Microsoft.EntityFrameworkCore;

namespace Notification.Api.Data;

public static class MigrationExtension
{
    public static async Task ApplyMigrationAsync(
        this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

        if (!await db.Database.EnsureCreatedAsync())
        {
            var pendingMigrations = await db.Database.GetPendingMigrationsAsync();
            if (!pendingMigrations.Any())
                await db.Database.MigrateAsync();
        }
    }
}