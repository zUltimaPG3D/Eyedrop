using Eyedrop.Helpers;
using Eyedrop.MineXplorer.Types.Sessions;
using Microsoft.AspNetCore.Http.Extensions;

namespace Eyedrop.MineXplorer.Middlewares;

internal class MexpValidityMiddleware
{
    private readonly RequestDelegate _next;
    
    public MexpValidityMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.HasValue ? context.Request.Path.Value : null;
        if (path == null || !path.StartsWith("/m/")) 
        {
            await _next(context);
            return;
        }
        
        var isInDataPath = path == "/m/m/d";
        var isInAuthPath = path == "/m/u/v";
        var ignoreMissingSessions = isInDataPath || isInAuthPath || path == "/m/m/c" || path == "/m/u/c";
        
        context.Items["Session"] = null;
        
        var me = context.GetHeaderSafe("me");
        if (me != null && me != "none")
        {
            var session = await SessionManager.CreateOrGetSession(me, true);
            if (session != null)
            {
                context.Items["Session"] = session;
                session.ResetTimer();
            }
        }
        
        if (context.Items["Session"] == null && !ignoreMissingSessions)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Unexpectedly, a crisis.");
            return;
        }
        
        await _next(context);
    }
}