using Eyedrop.MineXplorer.Middlewares;
using Microsoft.AspNetCore.Http.Extensions;

internal static class Program
{
    private static void Main(string[] args)
    {
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