using System.Text.Json;
using FluentValidation;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is ApiException apiException)
            {
                await HandleApiException(context, apiException);
            }
            else
            {
                await HandleUnknownException(context, exception);
            }
        }

        private async Task HandleApiException(HttpContext context, ApiException exception)
        {
            context.Response.StatusCode = (int)exception.StatusCode;
            var response = new
            {
                Status = context.Response.StatusCode,
                Error = exception.Errors
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private async Task HandleUnknownException(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            var response = new
            {
                Status = context.Response.StatusCode,
                Errors = new[] { "An internal server error occurred" }
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}