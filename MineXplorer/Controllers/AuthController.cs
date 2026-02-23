using Eyedrop.MineXplorer.Captchas;
using Eyedrop.Helpers;
using Microsoft.AspNetCore.Mvc;
using Eyedrop.MineXplorer.Types;
using Eyedrop.MineXplorer.Helpers;
using Eyedrop.MineXplorer.Types.Sessions;

namespace Eyedrop.MineXplorer.Controllers;

[ApiController]
public class AuthController : Controller
{
    [HttpGet("/m/m/c")]
    public async Task<IActionResult> CreateCaptcha()
    {
        var me = HttpContext.GetHeaderSafe("me");
        if (me == null || me != "none")
        {
            return ResponseHelper.RequestError();
        }
        var captcha = CaptchaManager.NewCaptcha();
        var image = await CaptchaImage.CreatePNG(captcha.Solution);
        return File(image, "image/png; charset=binary");
    }
    
    [HttpGet("/m/u/c")]
    public async Task<IActionResult> CreateUser()
    {
        var me = HttpContext.GetHeaderSafe("me");
        var userSolution = HttpContext.GetHeaderSafe("ca");
        if (me == null || me != "none" || userSolution == null)
        {
            return ResponseHelper.RequestError();
        }
        
        
        bool valid = CaptchaManager.CaptchaExists(userSolution);
        if (!valid) return Content("0");
        
        var user = await DatabaseHelper.CreateUser();
        if (user == null) return Content("0");
        
        return Content(user.FullUser);
    }
    
    [HttpGet("/m/u/v")]
    public async Task<IActionResult> ValidateUser()
    {
        var me = HttpContext.GetHeaderSafe("me");
        var version = HttpContext.GetHeaderSafe("vs");
        if (me == null || version != MineXplorerInfo.Version.ToString())
        {
            return ResponseHelper.RequestError();
        }
        
        var session = await SessionManager.CreateOrGetSession(me, false);
        return Content(session == null ? "" : "1");
    }
}
