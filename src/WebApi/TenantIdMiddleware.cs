namespace WebApi;

public class TenantIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantIdMiddleware> _logger;

    public TenantIdMiddleware(RequestDelegate next, ILogger<TenantIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!IsValidTenantId(context))
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Tenant header is not valid.");
            return;
        }

        await _next(context);
    }
    
    private static bool IsValidTenantId(HttpContext context)
    {
        if (context is null)
        {
            return false;
        }
        
        if (!context.Request.Headers.TryGetValue(Constants.TenantIdHeaderName, out var tenantId) || tenantId.Count != 1)
        {
            return false;
        }

        return tenantId.Single().IsValidTenantId(acceptNumericValues: false);
    }
}

public static class TenantIdMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantId(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        return app.UseMiddleware<TenantIdMiddleware>();
    }
}