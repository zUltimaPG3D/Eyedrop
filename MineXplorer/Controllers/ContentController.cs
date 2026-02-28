using System.Security.Cryptography;
using System.Text;
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
        return PhysicalFile(SpeakManager.WordlistPath, "text/plain; charset=utf-8");
    }
    
    [HttpGet($"/m/m/s")]
    public async Task<IActionResult> GetLastSpeak()
    {
        var lines = (await System.IO.File.ReadAllTextAsync(SpeakManager.SpeakPath)).TrimEnd();
        return File(Encoding.UTF8.GetBytes(lines), "text/plain; charset=utf-8");
    }
    
    [HttpGet($"/m/m/m")]
    public async Task<IActionResult> DownloadMap([FromHeader] string map, [FromHeader(Name = "sd")] string spawnData)
    {
        if (HttpContext.Items["Session"] is not Session session)
        {
            return Content("");
        }

        static string GetMapPath(string name)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Content", "Maps", "2022", name);
        }
        
        var checkPath = GetMapPath(map);
        
        // validity
        var wasInMap = session.User.LastSpawnData.Scene == map;
        var requiresTokenForMap = MineXplorerInfo.TokenRequirements.ContainsKey(map);
        var hasMapToken = requiresTokenForMap && session.User.Tokens.Any(x => x.ID == MineXplorerInfo.TokenRequirements[map] && x.Legitimate);
        var mapExists = MineXplorerInfo.AllMaps.Contains(map);
        
        var invalid = ((requiresTokenForMap && !hasMapToken) || !mapExists) && !wasInMap;
        
        if (map == "map_void_white" && !wasInMap)
        {
            map = "map_hell";
            spawnData = "0 0.9 0 0";
        }
        
        if (invalid)
        {
            map = "map_void";
            spawnData = "0 0.9 0 0";
        }
        
        session.User.LastSpawnData.Scene = map;
        session.User.LastSpawnData.Position = Vector4Converter.StringToVec(spawnData);
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
        
        var finalPath = GetMapPath(map);
        var data = await System.IO.File.ReadAllBytesAsync(finalPath);
        return File(data, "application/octet-stream; charset=binary");
    }
    
    [HttpGet($"/m/o/g")]
    public async Task<IActionResult> GetGhosts()
    {
        if (HttpContext.Items["Session"] is not Session session)
        {
            return Content("");
        }
        
        var map = session.User.LastSpawnData.Scene;
        var ghosts = await DatabaseHelper.AllGhostsIn(map);
        var query = ghosts.Where(x => x.Name != session.User.Username).Select(async x => await x.AsString());
        
        var split = string.Join('\n', await Task.WhenAll(query));
        
        return Content(split);
    }
    
    [HttpGet($"/m/o/p")]
    public async Task<IActionResult> GetPlayerCount()
    {
        return Content(SessionManager.PublicSessionCount.ToString());
    }
    
    [HttpGet($"/m/n/a")]
    public async Task<IActionResult> GetAnnouncement()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Content", "announcement.txt");
        return PhysicalFile(path, "text/plain; charset=utf-8");
    }
    
    [HttpGet($"/m/n/n")]
    public async Task<IActionResult> GetNews()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Content", "news.txt");
        return PhysicalFile(path, "text/plain; charset=utf-8");
    }
    
    [HttpGet($"/m/m/i")]
    public async Task<IActionResult> GetImage()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Content", "Images");
        var files = Directory.GetFiles(path);
        var random = files[RandomNumberGenerator.GetInt32(files.Length)];
        return PhysicalFile(random, "image/png; charset=binary");
    }
    
    [HttpGet($"/m/m/a")]
    public async Task<IActionResult> GetAd([FromHeader] string id)
    {
        var listPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Content", "adlist.txt");
        var listData = (await System.IO.File.ReadAllLinesAsync(listPath)).Where(x => !string.IsNullOrWhiteSpace(x));
        
        if (id == "0")
        {
            var listString = string.Join('\n', listData);
            return File(Encoding.UTF8.GetBytes(listString), "text/plain; charset=utf-8");
        }
        
        var isValid = listData.Any(x => x == id);
        if (!isValid) return ResponseHelper.RequestError();
        
        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Content", "Ads", $"{id}.png");
        if (!System.IO.File.Exists(imagePath)) return ResponseHelper.RequestError();
        
        return PhysicalFile(imagePath, "image/png; charset=binary");
    }
    
    [HttpGet($"/m/u/p")]
    public async Task<IActionResult> GetProfile([FromHeader(Name = "pr")] string? targetName)
    {
        if (HttpContext.Items["Session"] is not Session session)
        {
            return Content("");
        }
        
        if (targetName == "_edit") return Content("_edit\nDon't mess with it.\nalways\nall");
        if (targetName == null) return Content(await session.User.GenerateProfile());
        
        var user = await DatabaseHelper.GetUser(targetName);
        if (user != null) return Content(await user.GenerateProfile());
        
        return Content("\n\nundefined\nundefined");
    }
}
