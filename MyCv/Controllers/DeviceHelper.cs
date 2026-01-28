public static class DeviceHelper
{
    public static string GetOrCreateDeviceId(HttpContext context)
    {
        const string cookieName = "DeviceId";

        if (context.Request.Cookies.TryGetValue(cookieName, out var deviceId))
        {
            return deviceId;
        }

        deviceId = Guid.NewGuid().ToString();
        
        context.Response.Cookies.Append(
            cookieName,
            deviceId,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddYears(5)
            });

        return deviceId;
    }
}