using Serilog.Context;
using System.Diagnostics;

namespace MShop.API.Customer.Middlewares.Observability
{
    public class RequestContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestContextMiddleware> _logger;

        public RequestContextMiddleware(RequestDelegate next, ILogger<RequestContextMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = context.Request.Headers["X-Correlation-Id"]
                .FirstOrDefault() ?? Guid.NewGuid().ToString();

            context.Response.Headers["X-Correlation-Id"] = correlationId;

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("TraceId", context.TraceIdentifier))
            using (LogContext.PushProperty("Path", context.Request.Path))
            {
                await _next(context);
            }
        }
    }
}
