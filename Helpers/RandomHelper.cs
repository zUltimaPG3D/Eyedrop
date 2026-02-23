namespace Eyedrop.Helpers;

internal static class RandomHelper
{
    public enum RandomStringKind
    {
        Alphanumeric,
        AllCase,
        Lowercase,
    }
    
    private const string Alphanumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const string AllCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    
    public static float Single(float min, float max)
    {
        return min + Random.Shared.NextSingle() * (max - min);
    }
    
    public static bool Bool(float chanceIn100 = 50.0f)
    {
        return Single(0, 100) >= 100.0f - chanceIn100;
    }
    
    public static string String(int length, RandomStringKind kind = RandomStringKind.AllCase)
    {
        string chars = AllCase;
        if (kind == RandomStringKind.Alphanumeric) chars = Alphanumeric; 
        if (kind == RandomStringKind.Lowercase) chars = Lowercase; 
        return new string([.. Enumerable.Repeat(chars, length).Select(chars => chars[Random.Shared.Next(0, chars.Length)])]);
    }
    
    public static (char ch, int idx)[] Shuffle(string input)
    {
        return [.. input.Select((c, i) => (c, i)).OrderBy(_ => Random.Shared.Next())];
    }
}