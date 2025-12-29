using Microsoft.EntityFrameworkCore;
using Notification.Api.Entities;

namespace Notification.Api.Data;

public class NotificationDbContext(DbContextOptions<NotificationDbContext> options)
    : DbContext(options)
{
    public DbSet<ProcessedEvent> ProcessedEvents { get; set; }
}