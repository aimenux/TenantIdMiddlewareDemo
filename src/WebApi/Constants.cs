namespace WebApi;

public static class Constants
{
    public const string TenantIdHeaderName = "X-Tenant-Id";
    
    public enum Tenants
    {
        Fr = 1,
        Be,
        Es,
        De
    }
}