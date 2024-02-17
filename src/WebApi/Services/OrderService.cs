using AutoFixture;

using WebApi.Models;

namespace WebApi.Services;

public class OrderService : IOrderService
{
    private const int Delay = 1000;
    private static readonly Fixture Fixture = new();
    
    public async Task<IEnumerable<Order>> GetOrdersByTenantIdAsync(string tenantId, CancellationToken cancellationToken)
    {
        await Task.Delay(Delay, cancellationToken);
        var orders = Fixture
            .Build<Order>()
            .With(x => x.TenantId, tenantId)
            .CreateMany();
        return orders;
    }
}