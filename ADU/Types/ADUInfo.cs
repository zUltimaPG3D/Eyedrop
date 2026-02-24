namespace Eyedrop.ADU.Types;

internal static class ADUInfo
{
    public static readonly string BasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Eyedrop", "ADU");
    public const string Password = "AOD-Password";
    
    static ADUInfo()
    {
        if (!Directory.Exists(BasePath))
        {
            Directory.CreateDirectory(BasePath);
        }
    }
}