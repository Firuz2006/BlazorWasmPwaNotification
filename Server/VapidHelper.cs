using WebPush;

namespace Server;

public static class VapidHelper
{
    public static VapidDetails GenerateVapidKeys()
    {
        return WebPush.VapidHelper.GenerateVapidKeys();
    }
}