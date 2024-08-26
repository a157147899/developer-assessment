using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace TodoList.Api.Middlewares
{
    public class AuditLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditLoggingMiddleware> _logger;

        public AuditLoggingMiddleware(RequestDelegate next, ILogger<AuditLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Options)
            {
                await _next(context);
                return;
            }

            // Log the request details
            _logger.LogInformation($"Incoming request: {context.Request.Method} {context.Request.Path}");

            // Log the request body in JSON format
            context.Request.EnableBuffering();
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            _logger.LogInformation($"Request body: {requestBody}");

            var originalResponseBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                // Log the response details
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                _logger.LogInformation($"Response body: {responseBodyText}");

                // Copy the response back to the original response body stream
                await responseBody.CopyToAsync(originalResponseBodyStream);
            }
        }
    }
}
