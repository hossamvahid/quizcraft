using log4net;

namespace src.Presentation.Middlewares
{
    public class StartRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog _log;

        public StartRequestMiddleware(RequestDelegate next, ILog log)
        {
            _next = next;
            _log = log;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var forwardedFor = context.Request.Headers["X-Fowarded-For"].FirstOrDefault();
            var ipAddress = forwardedFor ?? context.Connection.RemoteIpAddress?.ToString();

            _log.Info($"Request {context.Request.Protocol} , {context.Request.Method} , {context.Request.Path} from IP : {ipAddress}");

            context.Response.Headers["Server"] = "Triplo";
            await _next(context);
        }
    }
}
