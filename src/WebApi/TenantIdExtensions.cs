using WebApi.Configuration;

namespace WebApi;

public static class TenantIdExtensions
{
    public static bool IsValidTenantId(this string tenantId, bool acceptNumericValues)
    {
        if (acceptNumericValues)
        {
            return Enum.TryParse(tenantId, true, out Constants.Tenants tenantIdEnum) &&
                   Enum.IsDefined(typeof(Constants.Tenants), tenantIdEnum);
        }
        
        var tenants = new HashSet<string>(Enum.GetNames(typeof(Constants.Tenants)), StringComparer.OrdinalIgnoreCase);
        return tenants.TryGetValue(tenantId, out _);
    }
}