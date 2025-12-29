using Microsoft.Extensions.Options;
using Policy.Infrastructure.Options;
using Shared.Events;

namespace Policy.Infrastructure.Messaging;

public class EventRouting(IOptions<EventRoutingOptions> options)
{
    private readonly Dictionary<string,string> _routingOptions = options.Value.Routes;

    public string GetQueueName(string eventType)
    {
        if (string.IsNullOrWhiteSpace(eventType))
            throw new ArgumentException("Event type shoudn`t be empty");

        if (!_routingOptions.TryGetValue(eventType, out var route))
            throw new InvalidOperationException("Cannot find event type");

        return route;
    }
}