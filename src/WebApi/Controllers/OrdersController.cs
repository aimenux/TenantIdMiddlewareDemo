using Microsoft.AspNetCore.Mvc;

using WebApi.Configuration;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrders([FromHeader(Name = Constants.TenantIdHeaderName)] string tenantId, CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetOrdersByTenantIdAsync(tenantId, cancellationToken);
        return Ok(orders);
    }
}