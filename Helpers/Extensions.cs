namespace Eyedrop.Helpers;

internal static class Extensions
{
    public static string? GetHeaderSafe(this HttpContext ctx, string key)
    {
        if (!ctx.Request.Headers.TryGetValue(key, out var values))
        {
            return null;
        }
        
        return ctx.Request.Headers.First(x => x.Key == key).Value;
    }
}