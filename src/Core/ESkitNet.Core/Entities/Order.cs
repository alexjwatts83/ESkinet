﻿namespace ESkitNet.Core.Entities;

public class Order : Aggregate<OrderId>
{
    public Order()
    {
        
    }

    public Order(List<OrderItem> items)
    {
        _orderItems = items;
    }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public required string BuyerEmail { get; set; }
    public ShippingAddress ShippingAddress { get; set; } = null!;
    public DeliveryMethod DeliveryMethod { get; set; } = null!;
    public PaymentSummary PaymentSummary { get; set; } = null!;

    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public decimal SubTotal { get; set; }
    public required string PaymentIntentId { get; set; }
}