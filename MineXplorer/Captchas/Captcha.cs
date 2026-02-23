namespace Eyedrop.MineXplorer.Captchas;

public class Captcha
{
    public readonly string Solution;
    public readonly DateTime Expiry;
    
    public Captcha(string solution)
    {
        Solution = solution;
        Expiry = DateTime.UtcNow.AddMinutes(5);
    }
}