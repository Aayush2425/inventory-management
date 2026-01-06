using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IMS_API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred while processing request");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var errorCode = "internal_error";
            var message = "An unexpected error occurred.";

            if (exception is InvalidOperationException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                errorCode = "invalid_operation";
                message = exception.Message;
            }

            else if (exception is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                errorCode = "unauthorized";
                message = exception.Message;
            }

            var problem = new
            {
                status = statusCode,
                error = errorCode,
                message,
                // traceId can help debugging in logs
                traceId = context.TraceIdentifier
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(problem);
            await context.Response.WriteAsync(json);
        }
    }
}
