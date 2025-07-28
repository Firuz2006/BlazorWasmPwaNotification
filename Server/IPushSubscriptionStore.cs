using WebPush;

namespace Server
{
    public interface IPushSubscriptionStore
    {
        Task AddAsync(PushSubscription subscription);
        Task RemoveAsync(string endpoint);
        Task<IEnumerable<PushSubscription>> GetAllAsync();
    }
}
