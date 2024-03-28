using Microsoft.AspNetCore.Http;

namespace Zio.Features.Core.Extensions;

public static class HttpContextExtension
{
    /// <summary>
    /// 判断是否是 WebSocket 请求
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static bool IsWebSocketRequest(this HttpContext context)
    {
        return context.WebSockets.IsWebSocketRequest || context.Request.Path == "/ws";
    }
}