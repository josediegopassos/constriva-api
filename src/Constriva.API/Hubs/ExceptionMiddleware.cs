using System.Text.Json;

namespace Constriva.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next; _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try { await _next(context); }
            catch (Exception ex) { await HandleExceptionAsync(context, ex); }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            context.Response.ContentType = "application/json";

            var (status, message) = ex switch
            {
                FluentValidation.ValidationException ve => (400, string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))),
                UnauthorizedAccessException => (401, ex.Message),
                KeyNotFoundException => (404, ex.Message),
                InvalidOperationException => (422, ex.Message),
                _ => (500, "Ocorreu um erro interno. Por favor, tente novamente.")
            };

            context.Response.StatusCode = status;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                statusCode = status,
                message,
                traceId = context.TraceIdentifier
            }));
        }
    }
}
