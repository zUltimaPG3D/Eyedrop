using System.Numerics;
using System.Text.Json;
using Eyedrop.Helpers;
using Microsoft.EntityFrameworkCore;
namespace Eyedrop.MineXplorer.Types;

internal class GameContext : DbContext
{
    public DbSet<Ghost> Ghosts { get; set; }
    public DbSet<User> Users { get; set; }
    
    public static string DbPath
    {
        get
        {
            var dataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var eyedropPath = Path.Combine(dataPath, "Eyedrop");
            if (!Directory.Exists(eyedropPath)) Directory.CreateDirectory(eyedropPath);
            return Path.Combine(eyedropPath, "minexplorer.db");
        }
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
    
    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<User>()
            .Property(u => u.Tokens)
            .HasConversion(
                v => JsonSerializer.Serialize(v),
                v => JsonSerializer.Deserialize<List<Token>>(v)!
            );
        
        model.Entity<User>()
            .Property(u => u.LastSpawnData)
            .HasConversion(
                v => v.ToString(),
                v => SpawnData.FromString(v)
            );
        
        model.Entity<User>()
            .Property(u => u.LastPlayed)
            .HasConversion(
                v => (ulong)new DateTimeOffset(v).ToUnixTimeSeconds(),
                v => DateTimeOffset.FromUnixTimeSeconds((long)v).UtcDateTime
            );
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Vector4>()
            .HaveConversion<Vector4Converter>();
    }
}