using System.Collections.Concurrent;
using Eyedrop.MineXplorer.Helpers;

namespace Eyedrop.MineXplorer.Types.Sessions;

internal static class SessionManager
{
    private static readonly List<Session> Sessions = [];
    private static readonly Lock ConcurrentLock = new();
    
    public static int PublicSessionCount
    {
        get
        {
            lock (ConcurrentLock) return Sessions.Count(x => x.Username != "_editor");
        }
    }
    
    public static void RemoveSession(Session session)
    {
        lock (ConcurrentLock) Sessions.Remove(session);
    }
    
    public static async Task<Session?> CreateOrGetSession(string user, bool disallowCreation = true)
    {
        lock (ConcurrentLock)
        {
            if (Sessions.Any(x => x.Username == user)) return Sessions.First(x => x.Username == user);
        }
        
        if (disallowCreation == true) return null;
        
        var gameUser = await DatabaseHelper.GetUser(user);
        if (gameUser == null) return null;
        
        var gameGhost = await DatabaseHelper.GetGhostAsync(user);
        
        var session = new Session
        {
            Username = user,
            User = gameUser,
            Ghost = gameGhost,
        };
        
        lock (ConcurrentLock) Sessions.Add(session);
        return session;
    }
}