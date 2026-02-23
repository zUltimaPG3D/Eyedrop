using Eyedrop.MineXplorer.Types;
using Microsoft.AspNetCore.Mvc;

namespace Eyedrop.MineXplorer.Controllers;

[ApiController]
public class InfoController : Controller
{
    [HttpGet("/m/m/d")]
    public async Task<IActionResult> DataPath()
    {
        return Content(MineXplorerInfo.DataPath);
    }
    
    [HttpGet($"/{MineXplorerInfo.DataPath}/allowed")]
    public async Task<IActionResult> PlayingAllowed()
    {
        return Content(MineXplorerInfo.PlayingAllowed ? "1" : "0");
    }
    
    [HttpGet($"/{MineXplorerInfo.DataPath}/version")]
    public async Task<IActionResult> GameVersion()
    {
        return Content(MineXplorerInfo.Version.ToString());
    }
}
