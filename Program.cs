using Eyedrop.MineXplorer.Middlewares;
using Eyedrop.MineXplorer.Types;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        using (var mexpDb = new GameContext())
        {
            await mexpDb.Database.MigrateAsync();
        }
        
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();

        var app = builder.Build();
        app.Use(async (ctx, next) =>
        {
            var request = ctx.Request;
            Console.WriteLine($"{ctx.Request.Method} request to {ctx.Request.GetDisplayUrl()}");
            await next();
        });
        app.UseMiddleware<UserMiddleware>();
        app.UseRouting();
        app.MapControllers();
        app.Run();
    }
}