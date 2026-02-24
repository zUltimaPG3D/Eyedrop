using Eyedrop.ADU.Types;
using Eyedrop.Helpers;
using Eyedrop.MineXplorer.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eyedrop.ADU.Controllers;

[ApiController]
public class PostController : Controller
{
    [HttpPost("/adu/set")]
    public async Task<IActionResult> UploadSomething(List<IFormFile> image, [FromForm] string password)
    {
        if (image.Count != 1) return ResponseHelper.RequestError();
        
        var file = image[0];
        if (file.Length > 1024 * 1024) return Content("File size limit has been reached");
        
        if (password != ADUInfo.Password) return Content("Incorrect password");
        
        if (!file.FileName.EndsWith(".png") && !file.FileName.EndsWith(".jpg") && !file.FileName.EndsWith(".gif")) return Content("Incorrect file format");
        
        var upload = new Upload
        {
            ID = RandomHelper.String(32, RandomHelper.RandomStringKind.Lowercase),
            TiedPath = file.FileName
        };
        
        var outPath = Path.Combine(ADUInfo.BasePath, file.FileName);
        using (var stream = System.IO.File.Create(outPath))
        {
            await file.CopyToAsync(stream);
        }
        
        using var db = new ADUContext();
        await db.Uploads.AddAsync(upload);
        await db.SaveChangesAsync();
        
        return Content($"Uploaded to ID {upload.ID}");
    }
    
    [HttpPost("/adu/cut")]
    public async Task<IActionResult> RemoveSomething([FromForm] string id, [FromForm] string password)
    {
        if (password != ADUInfo.Password) return Content("Incorrect password");
        
        using var db = new ADUContext();
        var exists = await db.Uploads.AnyAsync(x => x.ID == id);
        if (!exists) return Content("Please provide a valid ADU ID");
        
        var upload = await db.Uploads.FirstAsync(x => x.ID == id);
        
        var outPath = Path.Combine(ADUInfo.BasePath, upload.TiedPath);
        if (System.IO.File.Exists(outPath)) System.IO.File.Delete(outPath);
        
        db.Uploads.Remove(upload);
        await db.SaveChangesAsync();
        
        return Content($"The file at ID {id} has been removed");
    }
}