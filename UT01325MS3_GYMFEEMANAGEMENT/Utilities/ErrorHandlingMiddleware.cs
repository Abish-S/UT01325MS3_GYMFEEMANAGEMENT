using System.Net;
using System.Text.Json;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;

namespace UT01325MS3_GYMFEEMANAGEMENT.Utilities
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Proceed to the next middleware
            }
            catch (ArgumentException ex)
            {
                // Handle 400 Bad Request exceptions
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                // Handle 404 Not Found exceptions
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                // Handle 500 Internal Server Error exceptions
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new StandardErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Error = statusCode.ToString(), // Example: "BadRequest" or "InternalServerError"
                Message = message,
                Detail = statusCode == HttpStatusCode.InternalServerError ? "Contact support with this issue." : null,
                Instance = context.Request.Path
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
        }
    }
}
