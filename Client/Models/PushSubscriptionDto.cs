namespace Client.Models;

public record SubscriptionKeys(string p256dh, string auth);
public record PushSubscriptionDto(string endpoint, SubscriptionKeys keys);

