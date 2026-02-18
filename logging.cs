using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log incoming request
        var method = context.Request.Method;
        var path = context.Request.Path;

        _logger.LogInformation("Incoming Request: {Method} {Path}", method, path);

        // Call the next middleware in the pipeline
        await _next(context);

        // Log outgoing response
        var statusCode = context.Response.StatusCode;

        _logger.LogInformation("Outgoing Response: {StatusCode} for {Method} {Path}", 
            statusCode, method, path);
    }
}
