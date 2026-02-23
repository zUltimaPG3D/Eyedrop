using System.Security.Cryptography;
using Eyedrop.Helpers;
using Eyedrop.MineXplorer.Helpers;
using Eyedrop.MineXplorer.Types;
using Eyedrop.MineXplorer.Types.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace Eyedrop.MineXplorer.Controllers;

[ApiController]
public class ContentController : Controller
{
    [HttpGet($"/m/m/w")]
    public async Task<IActionResult> WordList()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Content", "wordlist.txt");
        var data = await System.IO.File.ReadAllBytesAsync(path);
        return File(data, "text/plain; charset=us-ascii");
    }
    
    [HttpGet($"/m/m/m")]
    public async Task<IActionResult> DownloadMap()
    {
        var map = HttpContext.GetHeaderSafe("map");
        var spawnData = HttpContext.GetHeaderSafe("sd");
        if (HttpContext.Items["Session"] is not Session session || map == null || spawnData == null)
        {
            return Content("");
        }
        
        session.User.LastSpawnData = $"{map} {spawnData}";
        await session.User.UpdateAsync();
        
        session.ResetMapTimer();
        if (map == "map_void" || map == "map_hell" || map == "map_void_white")
        {
            session.DoMapTimer("map_welcome");
        }
        else if (map == "map_maze")
        {
            session.DoMapTimer("map_hell");
        }
        
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Content", "Maps", "2022", map);
        var data = await System.IO.File.ReadAllBytesAsync(path);
        return File(data, "application/octet-stream; charset=binary");
    }
    
    [HttpGet($"/m/o/g")]
    public async Task<IActionResult> GetGhosts()
    {
        if (HttpContext.Items["Session"] is not Session session)
        {
            return Content("");
        }
        
        var map = session.User.LastSpawnData.Split(" ")[0];
        var ghosts = await DatabaseHelper.AllGhostsIn(map);
        var split = string.Join('\n', ghosts.Where(x => x.Name != session.User.Username).Select(x => x.ToString()));
        
        return Content(split);
    }
    
    [HttpGet($"/m/o/p")]
    public async Task<IActionResult> GetPlayerCount()
    {
        var me = HttpContext.GetHeaderSafe("me");
        if (me == null)
        {
            return Content("");
        }
        
        return Content(SessionManager.PublicSessionCount.ToString());
    }
    
    [HttpGet($"/m/n/a")]
    public async Task<IActionResult> GetAnnouncement()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Content", "announcement.txt");
        var data = await System.IO.File.ReadAllBytesAsync(path);
        return File(data, "text/plain; charset=us-ascii");
    }
    
    [HttpGet($"/m/n/n")]
    public async Task<IActionResult> GetNews()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Content", "news.txt");
        var data = await System.IO.File.ReadAllBytesAsync(path);
        return File(data, "text/plain; charset=us-ascii");
    }
    
    [HttpGet($"/m/m/i")]
    public async Task<IActionResult> GetImage()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Content", "Images");
        var files = Directory.GetFiles(path);
        var random = files[RandomNumberGenerator.GetInt32(files.Length)];
        
        var data = await System.IO.File.ReadAllBytesAsync(random);
        return File(data, "image/png; charset=binary");
    }
}
