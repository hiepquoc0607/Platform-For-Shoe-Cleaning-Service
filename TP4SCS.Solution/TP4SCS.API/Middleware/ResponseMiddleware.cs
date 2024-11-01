using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace TP4SCS.API.Middleware
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            // Check if the response status is 403
            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    status = "error",
                    statusCode = 403,
                    message = "Truy cập bị từ chối!"
                }));
            }
        }
    }

}
