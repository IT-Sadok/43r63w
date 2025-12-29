using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Policy.Application.Options;
using Policy.Infrastructure.Data;
using Policy.Infrastructure.Interfaces;

namespace Policy.Application.Services;

public class OutboxProcessor(
    IServiceProvider serviceProvider,
    ILogger<OutboxProcessor> logger,
    IOptions<OutboxProcessorOptions> options) : BackgroundService
{
    private readonly OutboxProcessorOptions _outboxProcessorOptions = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Starting OutboxProcessor processing");

            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PolicyDbContext>();

            var messages = await db.OutboxMessages
                .Where(m => m.ProcessedOn == null || m.Error != null)
                .OrderBy(a => a.OccurredOn)
                .Take(_outboxProcessorOptions.BatchSize)
                .ToListAsync(cancellationToken: stoppingToken);
            
            var publisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

            foreach (var message in messages)
            {
                try
                {
                    await publisher.PublishAsync(
                        message.Type,
                        message.Content,
                        stoppingToken);
                    
                    await db.OutboxMessages
                        .Where(e => e.Id == message.Id)
                        .ExecuteUpdateAsync(a =>
                            a.SetProperty(e => e.ProcessedOn, DateTime.Now)
                                .SetProperty(e => e.Error, (string?)null),
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