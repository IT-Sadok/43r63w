using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Policy.Domain.Entities;
using Policy.Infrastructure.Data;
using Policy.Infrastructure.Interfaces;

namespace Policy.Application.Services;

public class OutboxPublisherService(
    IEventPublisher publisher,
    IServiceProvider serviceProvider,
    ILogger<OutboxPublisherService> logger) : BackgroundService
{
    private const int BatchSize = 20;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Starting OutboxPublisherService processing");

            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PolicyDbContext>();

            var messages = await db.OutboxMessages
                .Where(m => m.ProcessedOn == null)
                .OrderBy(a => a.OccurredOn)
                .Take(BatchSize)
                .ToListAsync(cancellationToken: stoppingToken);

            foreach (var message in messages)
            {
                try
                {
                    await publisher.PublishAsync(
                        message.Type,
                        message.Content,
                        message.QueueName,
                        stoppingToken);
                    
                    await db.OutboxMessages.ExecuteUpdateAsync(a =>
                            a.SetProperty(e => e.ProcessedOn, DateTime.Now),
                        cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    await db.OutboxMessages.ExecuteUpdateAsync(a =>
                            a.SetProperty(e => e.ProcessedOn, DateTime.Now)
                                .SetProperty(e => e.Error, ex.Message),
                        cancellationToken: stoppingToken);
                }
            }
        }
    }
}