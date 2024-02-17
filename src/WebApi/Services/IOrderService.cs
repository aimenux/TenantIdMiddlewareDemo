using WebApi.Models;

namespace WebApi.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetOrdersByTenantIdAsync(string tenantId, CancellationToken cancellationToken);
}