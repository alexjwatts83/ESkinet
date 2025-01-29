namespace ESkitNet.Core.Entities;

public class ShoppingCart
{
    public required string Id { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = [];

    public DeliveryMethodId? DeliveryMethodId { get; set; }
    public string? ClientSecret { get; set; }
    public string? PaymentIntentId { get; set; }
}
