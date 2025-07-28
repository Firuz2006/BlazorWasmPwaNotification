using WebPush;

namespace Server
{
    public static class VapidHelper
    {
        public static VapidDetails GenerateVapidKeys() => WebPush.VapidHelper.GenerateVapidKeys();
    }
}
