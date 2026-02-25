using Eyedrop.MineXplorer.Captchas;
using Eyedrop.Helpers;
using Microsoft.AspNetCore.Mvc;
using Eyedrop.MineXplorer.Types;
using Eyedrop.MineXplorer.Types.Sessions;
using Eyedrop.MineXplorer.Helpers;

namespace Eyedrop.MineXplorer.Controllers;

[ApiController]
public class UserController : Controller
{
    [HttpGet("/m/u/t")]
    public async Task<IActionResult> GetUserTokens()
    {
        var session = (HttpContext.Items["Session"] as Session)!;
        
        return Content(string.Join(' ', session.User.Tokens.Select(x => x.ID)));
    }
    
    [HttpGet("/m/u/s")]
    public async Task<IActionResult> GetSpawnData()
    {
        if (HttpContext.Items["Session"] is not Session session)
        {
            return ResponseHelper.RequestError();
        }
        
        return Content(session.User.LastSpawnData.ToString());
    }
    
    [HttpGet("/m/u/g")]
    public async Task<IActionResult> UpdateGhost([FromHeader(Name = "gh")] string ghostData)
    {
        if (HttpContext.Items["Session"] is not Session session)
        {
            return ResponseHelper.RequestError();
        }
        
        session.CreateGhost();
        session.Ghost?.Scene = session.User.LastSpawnData.Scene;
        session.Ghost?.Position = Vector4Converter.StringToVec(ghostData);
        session.User.LastPlayed = DateTime.UtcNow;
        await session.Ghost?.UpdateAsync()!;
        await session.User.UpdateAsync();
        
        return Content("1");
    }
    
    [HttpGet("/m/o/t")]
    public async Task<IActionResult> ObtainToken([FromHeader(Name = "tk")] string token)
    {
        if (HttpContext.Items["Session"] is not Session session)
        {
            return ResponseHelper.RequestError();
        }
        
        if (!MineXplorerInfo.AllTokens.Contains(token)) return Content("0");
        
        var legitimate = true;
        var userMap = session.User.LastSpawnData.Scene;
        var neededMap = MineXplorerInfo.TokenMap[token];
        if (userMap != neededMap) legitimate = false;
        
        session.User.Tokens.Add(new Token(token, legitimate));
        await session.User.UpdateAsync();
        
        return Content("1");
    }
}
