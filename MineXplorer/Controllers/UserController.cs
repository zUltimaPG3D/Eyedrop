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
    
    [HttpGet("/m/o/s")]
    public async Task<IActionResult> SendSpeak([FromHeader(Name = "sp")] string speakIndices)
    {
        if (HttpContext.Items["Session"] is not Session session)
        {
            return ResponseHelper.RequestError();
        }
        
        var indices = speakIndices.Split(" ").Select(x => Convert.ToInt32(x)).ToArray();
        if (indices.Length < 1 || indices.Length > 26) return Content("");
        
        var converted = SpeakManager.ConvertIndicesToString(indices);
        
        SpeakManager.AddLine($"{session.User.Username}: {converted}");
        if (SpeakManager.LineAmount() > 25)
        {
            SpeakManager.RemoveTopLine();
        }
        
        session.Ghost?.LastSpeak = converted;
        session.Ghost?.UpdateAsync();
        
        return Content("1");
    }
}
