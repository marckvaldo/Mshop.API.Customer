using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Mshop.API.Customer.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //isso serve para propagar o erro no opentelemetry
                Activity.Current?.RecordException(ex);
                Activity.Current?.SetStatus(ActivityStatusCode.Error);

                var traceId = Activity.Current?.TraceId.ToString();

                _logger.LogError(ex,
                   "Erro não tratado | CorrelationId: {CorrelationId} | Path: {Path} | Method: {Method}",
                   traceId,
                   context.Request.Path,
                   context.Request.Method);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    traceId,
                    errors = _env.IsDevelopment()
                        ? new[] { ex.Message }
                        : new[] { "Erro interno do servidor" }
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
