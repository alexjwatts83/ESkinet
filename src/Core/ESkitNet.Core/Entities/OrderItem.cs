namespace ESkitNet.Core.Entities;

public class OrderItem : Entity<OrderItemId>
{
    public OrderId OrderId { get; private set; } = default!;
    public ProductItemOrdered ItemOrdered { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
