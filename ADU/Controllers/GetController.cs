using System.Net.Mime;
using Eyedrop.ADU.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace Eyedrop.ADU.Controllers;

[ApiController]
public class GetController : Controller
{
    private readonly FileExtensionContentTypeProvider _contentTypeProvider;

    public GetController()
    {
        _contentTypeProvider = new FileExtensionContentTypeProvider();
    }

    [HttpGet("/adu")]
    public async Task<IActionResult> Index()
    {
        return Content("deprecated, subject to removal<br>usage:<br>/get<br>/set<br>/cut<br>/sum", "text/html; charset=utf-8");
    }
    
    [HttpGet("/adu/get")]
    public async Task<IActionResult> GetImage([FromQuery] string? id)
    {
        if (id == null) return Content("Please provide a valid ADU ID");
        
        using var db = new ADUContext();
        var exists = await db.Uploads.AnyAsync(x => x.ID == id);
        if (!exists) return Content("Please provide a valid ADU ID");
        
        var upload = await db.Uploads.FirstAsync(x => x.ID == id);
        var path = await upload.GetAndConfirmFilePathAsync();
        if (path == null) return Content("Please provide a valid ADU ID");
        
        if (!_contentTypeProvider.TryGetContentType(path, out var contentType))
        {
            contentType = "application/octet-stream"; // fallback
        }
        
        return PhysicalFile(path, contentType);
    }
    
    [HttpGet("/adu/set")]
    public async Task<IActionResult> GetUploadPage()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Content", "Site", "ADU", "set.html");
        return PhysicalFile(path, "text/html; charset=utf-8");
    }
    
    [HttpGet("/adu/cut")]
    public async Task<IActionResult> GetRemovalPage()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Content", "Site", "ADU", "cut.html");
        return PhysicalFile(path, "text/html; charset=utf-8");
    }
    
    [HttpGet("/adu/sum")]
    public async Task<IActionResult> GetUploadCount()
    {
        using var db = new ADUContext();
        var count = await db.Uploads.CountAsync();
        return Content(count.ToString(), "application/json; charset=utf-8");
    }
}