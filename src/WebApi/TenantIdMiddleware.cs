using Microsoft.Extensions.Options;

using WebApi.Configuration;

namespace WebApi;

public class TenantIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IOptions<Settings> _options;
    private readonly HashSet<string> _excludedPaths;

    public TenantIdMiddleware(RequestDelegate next, IOptions<Settings> options)
    {
        _next = next;
        _options = options;
        _excludedPaths = GetExcludedPaths();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (IsExcludedPath(context))
        {
            await _next(context);
            return;
        }
        
        if (!IsValidTenantId(context))
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Tenant header is not valid.");
            return;
        }

        await _next(context);
    }
    
    private bool IsValidTenantId(HttpContext context)
    {
        if (context is null)
        {
            return false;
        }
        
        if (!context.Request.Headers.TryGetValue(Constants.TenantIdHeaderName, out var tenantId) || tenantId.Count != 1)
        {
            return false;
        }

        return tenantId.Single().IsValidTenantId(acceptNumericValues: _options.Value.AcceptNumericValues);
    }

    private bool IsExcludedPath(HttpContext context)
    {
        var segments = context.Request.Path.Value?.Split("/") ?? Array.Empty<string>();
        return segments.Any(x => _excludedPaths.Contains(x));
    }

    private HashSet<string> GetExcludedPaths() => new(_options.Value.ExcludedPaths, StringComparer.OrdinalIgnoreCase);
}

public static class TenantIdMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantId(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        return app.UseMiddleware<TenantIdMiddleware>();
    }
}