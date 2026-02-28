using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Eyedrop.MineXplorer.Types;

public class User
{
    [Key] public ulong ID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    public bool Banned { get; set; }
    public DateTime LastPlayed { get; set; }
    
    public List<Token> Tokens { get; set; } = [];
    public SpawnData LastSpawnData { get; set; } = new SpawnData();
    public Ghost Ghost { get; set; }
    
    [NotMapped]
    public string FullUser
    {
        get
        {
            return $"{Username}{Password}";
        }
    }
    
    public async Task UpdateAsync()
    {
        using var db = new GameContext();
        db.Update(this);
        await db.SaveChangesAsync();
    }
    
    public string GetTimeSinceLastOnline()
    {
        // TODO: implement the whole super complex annoying date thing
        return "just now";
    }
    
    public async Task<string> GenerateProfile()
    {
        Ghost? ghost = null;
        using (var db = new GameContext())
        {
            ghost = await db.Ghosts.FirstOrDefaultAsync(x => x.Name == Username);
        }
        
        var sb = new StringBuilder();
        sb.AppendLine(Username);
        sb.AppendLine((ghost == null || string.IsNullOrWhiteSpace(ghost.LastSpeak)) ? "" : $"'{ghost.LastSpeak}'");
        sb.AppendLine(GetTimeSinceLastOnline());
        sb.AppendLine(MineXplorerInfo.Version > 29 ? (Tokens.Count == 0 ? "(none)" : string.Join(", ", Tokens.Select(x => x.ID))) : Tokens.Count.ToString());
        return sb.ToString();
    }
}