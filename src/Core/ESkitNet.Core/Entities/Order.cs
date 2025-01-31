namespace ESkitNet.Core.Entities;

public class Order : Aggregate<OrderId>
{
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public required string BuyerEmail { get; set; }
    public ShippingAddress ShippingAddress { get; set; } = null!;
    public DeliveryMethod DeliveryMethod { get; set; } = null!;
    public PaymentSummary PaymentSummary { get; set; } = null!;

    public List<OrderItem> OrderItems { get; set; } = [];
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal SubTotal { get; set; }
    public required string PaymentIntentId { get; set; }

    public decimal Total()
    {
        return SubTotal + DeliveryMethod.Price;
    }

    public static Order Create(DeliveryMethod deliveryMethod,
        ShippingAddress shippingAddress, PaymentSummary paymentSummary,
        string paymentIntentId, string email, List<OrderItem> items)
    {
        var order = new Order()
        {
            Id = OrderId.Of(Guid.NewGuid()),
            //OrderDate = timeProvider.Now,
            DeliveryMethod = deliveryMethod,
            ShippingAddress = shippingAddress,
            SubTotal = items.Sum(x => x.Quantity * x.Price),
            PaymentSummary = paymentSummary,
            PaymentIntentId = paymentIntentId,
            BuyerEmail = email,
            OrderItems = items,
        };

        return order;
    }
}