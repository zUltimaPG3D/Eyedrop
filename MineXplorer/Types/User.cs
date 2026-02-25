using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
}