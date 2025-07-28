using System.Collections.Concurrent;
using WebPush;

namespace Server;

public class PushSubscriptionStore : IPushSubscriptionStore
{
    private readonly ConcurrentDictionary<string, PushSubscription> _subscriptions = new();

    public Task AddAsync(PushSubscription subscription)
    {
        _subscriptions[subscription.Endpoint] = subscription;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string endpoint)
    {
        _subscriptions.TryRemove(endpoint, out _);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<PushSubscription>> GetAllAsync()
    {
        return Task.FromResult(_subscriptions.Values.AsEnumerable());
    }
}