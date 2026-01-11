using Microsoft.Extensions.DependencyInjection;
using Shared.InMemoryQueue;

namespace Policy.Application.BackgroundService;

public sealed class EventDispatcher : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly InMemoryEventBus _bus;

    public EventDispatcher(IServiceProvider serviceProvider, InMemoryEventBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var @event in _bus.Reader.ReadAllAsync(stoppingToken))
        {
            using var scope = _serviceProvider.CreateScope();

            var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());

            var handlers = scope.ServiceProvider.GetServices(handlerType);
            foreach (dynamic handler in handlers)
            {
                if (handler is null)
                    throw new NullReferenceException($"Handler {handlerType} was null");

                await handler.HandleAsync((dynamic)@event);
            }
        }
    }
}