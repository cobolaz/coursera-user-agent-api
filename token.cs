using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenValidationMiddleware> _logger;

    public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Extract token from Authorization header
        var authHeader = context.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            await RespondUnauthorized(context);
            return;
        }

        var token = authHeader.Substring("Bearer ".Length);

        // Validate token (replace with your real validation logic)
        if (!IsTokenValid(token))
        {
            _logger.LogWarning("Invalid token received.");
            await RespondUnauthorized(context);
            return;
        }

        // Token is valid → continue pipeline
        await _next(context);
    }

    private bool IsTokenValid(string token)
    {
        // TODO: Replace with real validation (JWT validation, signature check, etc.)
        return token == "my-valid-token"; 
    }

    private static Task RespondUnauthorized(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        var payload = "{\"error\": \"Unauthorized access. Invalid or missing token.\"}";
        return context.Response.WriteAsync(payload);
    }
}
