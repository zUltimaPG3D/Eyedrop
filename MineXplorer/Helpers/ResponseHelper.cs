using Microsoft.AspNetCore.Mvc;

namespace Eyedrop.MineXplorer.Helpers;

internal static class ResponseHelper
{
    public static ObjectResult RequestError()
    {
        return new ObjectResult("Unexpectedly, a crisis.")
        {
            StatusCode = 500
        };
    }
}