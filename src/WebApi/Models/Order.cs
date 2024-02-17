namespace WebApi.Models;

public record Order
{
    public string OrderId { get; init; }
    public string TenantId { get; init; }
    public string CustomerId { get; init; }
    public decimal TotalPrice { get; init; }
}