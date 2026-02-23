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
        var me = HttpContext.GetHeaderSafe("me");
        if (me == null)
        {
            return StatusCode(404, "Unexpectedly, a crisis.");
        }
        
        return Content("");
    }
    
    [HttpGet("/m/u/s")]
    public async Task<IActionResult> GetSpawnData()
    {
        if (HttpContext.Items["Session"] is not Session session)
        {
            return ResponseHelper.RequestError();
        }
        
        return Content(session.User.LastSpawnData);
    }
    
    [HttpGet("/m/u/g")]
    public async Task<IActionResult> UpdateGhost()
    {
        var ghostData = HttpContext.GetHeaderSafe("gh");
        if (HttpContext.Items["Session"] is not Session session || ghostData == null)
        {
            return ResponseHelper.RequestError();
        }
        
        session.CreateGhost();
        session.Ghost?.Scene = session.User.LastSpawnData.Split(" ")[0];
        session.Ghost?.Position = Vector4Converter.StringToVec(ghostData);
        session.User.LastPlayed = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        await session.Ghost?.UpdateAsync()!;
        await session.User.UpdateAsync();
        
        return Content("1");
    }
}
