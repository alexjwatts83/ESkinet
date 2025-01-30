namespace ESkitNet.Core.Entities;

public class Order : Aggregate<OrderId>
{
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public required string BuyerEmail { get; set; }
    public ShippingAddress ShippingAddress { get; set; } = null!;
    public DeliveryMethod DeliveryMethod { get; set; } = null!;
    public PaymentSummary PaymentSummary { get; set; } = null!;

    public List<OrderItem> OrderItems { get; set; } = [];
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public decimal SubTotal { get; set; }
    public required string PaymentIntentId { get; set; }

    public decimal Total()
    {
        return SubTotal + DeliveryMethod.Price;
    }
}