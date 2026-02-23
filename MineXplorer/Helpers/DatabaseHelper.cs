using System.Text.RegularExpressions;
using Eyedrop.Helpers;
using Eyedrop.MineXplorer.Types;
using Microsoft.EntityFrameworkCore;

namespace Eyedrop.MineXplorer.Helpers;

internal partial class DatabaseHelper
{
    public static async Task<User?> CreateUser(string? username = null)
    {
        username ??= RandomHelper.String(64, RandomHelper.RandomStringKind.Lowercase);
        if (username.Length != 64 || !UsernameValidity().IsMatch(username))
        {
            return null;
        }
        
        var user = username[..5];
        var pass = username[5..];

        var newUser = new User
        {
            Username = user,
            Password = pass
        };

        using var db = new GameContext();
        await db.AddAsync(newUser);
        await db.SaveChangesAsync();
        return newUser;
    }
    
    public static async Task<User?> GetUser(string username)
    {
        using var db = new GameContext();
        var onlyUsername = username[..5];
        var user = await db.Users.FirstOrDefaultAsync(x => x.Username == onlyUsername);
        return user;
    }
    
    public static async Task<Ghost?> GetGhostAsync(string user)
    {   
        using var db = new GameContext();
        var username = user[..5];
        return await db.Ghosts.FirstOrDefaultAsync(x => x.Name == username);
    }
    
    public static async Task<List<Ghost>> AllGhostsIn(string scene)
    {
        using var db = new GameContext();
        return await db.Ghosts.Where(x => x.Scene == scene).ToListAsync();
    }

    [GeneratedRegex(@"^[a-z]+$")]
    private static partial Regex UsernameValidity();
}