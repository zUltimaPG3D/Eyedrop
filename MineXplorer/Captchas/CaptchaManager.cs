using System.Collections.Concurrent;
using Eyedrop.Helpers;

namespace Eyedrop.MineXplorer.Captchas;

public static class CaptchaManager
{
    private static readonly List<Captcha> Captchas = [];
    private static readonly Timer ClearTimer;
    private static readonly Lock ConcurrentLock = new();
    
    static CaptchaManager()
    {
        ClearTimer = new Timer(_ => ClearOldCaptchas(), null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
    }
    
    private static void ClearOldCaptchas()
    {
        lock (ConcurrentLock)
        {
            var now = DateTime.UtcNow;
            Captchas.RemoveAll(x => x.Expiry <= now);
        }
    }
    
    public static bool CaptchaExists(string solution)
    {
        lock (ConcurrentLock)
        {
            return Captchas.Any(x => x.Solution == solution);
        }
    }
    
    public static Captcha NewCaptcha()
    {
        var solution = RandomHelper.String(8, RandomHelper.RandomStringKind.Alphanumeric);
        var captcha = new Captcha(solution);
        lock (ConcurrentLock)
        {
            Captchas.Add(captcha);
        }
        return captcha;
    }
}