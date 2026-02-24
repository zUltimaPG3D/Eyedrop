using System.ComponentModel.DataAnnotations;

namespace Eyedrop.ADU.Types;

public class Upload
{
    [Key] public string ID { get; set; }
    public string TiedPath { get; set; }
    
    public async Task<string?> GetAndConfirmFilePathAsync()
    {
        if (TiedPath == null) return null;
        
        var checkPath = Path.Combine(ADUInfo.BasePath, TiedPath);
        if (!File.Exists(checkPath)) return null;
        
        return checkPath;
    }
}