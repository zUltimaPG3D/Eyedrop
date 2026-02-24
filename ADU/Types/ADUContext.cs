using Microsoft.EntityFrameworkCore;

namespace Eyedrop.ADU.Types;

internal class ADUContext : DbContext
{
    public DbSet<Upload> Uploads { get; set; }
    
    public static string DbPath
    {
        get
        {
            var dataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var eyedropPath = Path.Combine(dataPath, "Eyedrop");
            if (!Directory.Exists(eyedropPath)) Directory.CreateDirectory(eyedropPath);
            return Path.Combine(eyedropPath, "adu.db");
        }
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
}