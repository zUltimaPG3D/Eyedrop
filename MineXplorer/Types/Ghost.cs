using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Eyedrop.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Eyedrop.MineXplorer.Types;

public enum GhostType
{
    Classic,
    Authorized,
    Inactive,
    Upgrade,
    It,
    Gone,
}

public class Ghost
{
    [Key] public string Name { get; set; }
    public GhostType Type { get; set; }
    public string Scene { get; set; }
    public Vector4 Position { get; set; }
    public string? LastSpeak { get; set; }
    
    public async Task<GhostType> GetGhostType()
    {
        if (Type == GhostType.It && MineXplorerInfo.Version < 29) return GhostType.Classic;
        if (Type != GhostType.Classic) return Type;
        
        using var db = new GameContext();
        var user = await db.Users.FirstOrDefaultAsync(x => x.Username == Name); 
        
        if (user == null) return Type;
        var now = DateTime.UtcNow;
        
        var monthDiff = (now.Year - user.LastPlayed.Year) * 12 + now.Month - user.LastPlayed.Month;
        var weekDiff = (int)((now - user.LastPlayed).TotalDays / 7);
        
        Console.WriteLine(monthDiff);
        Console.WriteLine(weekDiff);
        
        if (monthDiff >= 8 && MineXplorerInfo.Version >= 37) return GhostType.Gone;
        if (weekDiff >= 2) return GhostType.Inactive;
        return GhostType.Classic;
    }
    
    public async Task UpdateAsync()
    {
        using var db = new GameContext();
        var exists = await db.Ghosts.AnyAsync(x => x.Name == Name);
        
        if (!exists)
            await db.Ghosts.AddAsync(this);
        else
            db.Ghosts.Update(this);
        
        await db.SaveChangesAsync();
    }
    
    public async Task<string> AsString()
    {
        var type = await GetGhostType();
        return $"{Vector4Converter.VecToString(Position)} {type} {Name} {LastSpeak?.Replace(" ", "@")}";
    }
}