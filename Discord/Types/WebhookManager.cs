using System.Net.Http;
using System.Text;
using System.Text.Json;

internal static class WebhookManager
{
    private static readonly HttpClient Client = new();
    
    public static async Task SendText(string URL, string Content)
    {
        if (string.IsNullOrWhiteSpace(URL)) return;
        
        using StringContent jsonContent = new(
            JsonSerializer.Serialize(new
            {
                content = Content
            }),
            Encoding.UTF8,
            "application/json");
        using var response = await Client.PostAsync(URL, jsonContent);
    }
}