using Eyedrop.Helpers;
using Eyedrop.MineXplorer.Types.Sessions;

namespace Eyedrop.MineXplorer.Middlewares;

internal class UserMiddleware
{
    private readonly RequestDelegate _next;
    
    public UserMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
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
        
        await _next(context);
    }
}