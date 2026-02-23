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
    
    public override string ToString()
    {
        return $"{Vector4Converter.VecToString(Position)} {Type} {Name} {LastSpeak?.Replace(" ", "@")}";
    }
}