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
                var correlationId = context.Items["CorrelationId"]?.ToString()
                            ?? context.TraceIdentifier;

                _logger.LogError(ex,
                   "Erro não tratado | CorrelationId: {CorrelationId} | Path: {Path} | Method: {Method}",
                   correlationId,
                   context.Request.Path,
                   context.Request.Method);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    traceId = context.TraceIdentifier,
                    errors = new[] { "Erro interno do servidor" }
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
