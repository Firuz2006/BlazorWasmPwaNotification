using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebPush;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PushController : ControllerBase
{
    private readonly WebPushClient _client;
    private readonly IPushSubscriptionStore _store;
    private readonly VapidDetails _vapidKeys;

    public PushController(IPushSubscriptionStore store, WebPushClient client, VapidDetails vapidKeys)
    {
        _store = store;
        _client = client;
        _vapidKeys = vapidKeys;
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] PushSubscriptionDto dto)
    {
        var subscription = new PushSubscription(dto.endpoint, dto.keys.p256dh, dto.keys.auth);
        await _store.AddAsync(subscription);
        return Ok();
    }

    [HttpPost("unsubscribe")]
    public async Task<IActionResult> Unsubscribe([FromBody] string endpoint)
    {
        await _store.RemoveAsync(endpoint);
        return Ok();
    }

    [HttpPost("push")]
    public async Task<IActionResult> Push([FromBody] NotificationRequest request)
    {
        var subs = await _store.GetAllAsync();
        var payload = JsonSerializer.Serialize(new { title = request.Title, body = request.Body });
        var vapidDetails = new VapidDetails("mailto:example@example.com", _vapidKeys.PublicKey, _vapidKeys.PrivateKey);
        foreach (var sub in subs)
            try
            {
                await _client.SendNotificationAsync(sub, payload, vapidDetails);
            }
            catch
            {
                // ignore failures
            }

        return Ok();
    }

    public class PushSubscriptionDto
    {
        public string endpoint { get; set; } = default!;
        public SubscriptionKeys keys { get; set; } = default!;
    }

    public class SubscriptionKeys
    {
        public string p256dh { get; set; } = default!;
        public string auth { get; set; } = default!;
    }

    public class NotificationRequest
    {
        public string Title { get; set; } = default!;
        public string Body { get; set; } = default!;
    }
}