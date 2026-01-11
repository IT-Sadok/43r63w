using System.Threading.Channels;

namespace Shared.InMemoryQueue;

public class InMemoryEventBus : IEventBus
{
    private readonly Channel<IEvent> _channel;

    public ChannelReader<IEvent> Reader => _channel.Reader;

    public InMemoryEventBus()
    {
        _channel = Channel.CreateUnbounded<IEvent>();
    }

    public async Task PublishAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default) where TEvent : IEvent
    {
        await _channel.Writer.WriteAsync(@event, cancellationToken);
    }
}