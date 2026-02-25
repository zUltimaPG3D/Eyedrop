using System.Numerics;
using System.Timers;

namespace Eyedrop.MineXplorer.Types.Sessions;

internal class Session
{
    public string Username;
    public User User;
    public Ghost? Ghost;
    
    private readonly System.Timers.Timer InactivityTimer;
    private System.Timers.Timer MapChangeTimer;
    
    public Session()
    {
        InactivityTimer = new System.Timers.Timer(TimeSpan.FromMinutes(1));
        InactivityTimer.Elapsed += BecomeInactive;
        InactivityTimer.AutoReset = false;
        InactivityTimer.Enabled = true;
    }
    
    public void ResetTimer()
    {
        InactivityTimer.Stop();
        InactivityTimer.Start();
    }
    
    public void ResetMapTimer()
    {
        MapChangeTimer?.Stop();
        MapChangeTimer?.Dispose();
    }
    
    public void DoMapTimer(string target)
    {
        MapChangeTimer?.Dispose();
        MapChangeTimer = new System.Timers.Timer(TimeSpan.FromMinutes(4));
        MapChangeTimer.Elapsed += async (_, _) => await ChangeMap(target);
        MapChangeTimer.AutoReset = false;
        MapChangeTimer.Enabled = true;
    }
    
    private async Task ChangeMap(string target)
    {
        User.LastSpawnData.Scene = target;
        User.LastSpawnData.Position = SpawnData.DefaultPosition;
        await User.UpdateAsync();
    }
    
    private void BecomeInactive(object? source, ElapsedEventArgs e)
    {
        Console.WriteLine($"{Username} is inactive!");
        SessionManager.RemoveSession(this);
    }
    
    public void CreateGhost()
    {
        Ghost ??= new Ghost
        {
            Name = Username[..5],
            Type = GhostType.Classic,
            Position = new Vector4(0, 0.9f, 0, 0)
        };
    }
}