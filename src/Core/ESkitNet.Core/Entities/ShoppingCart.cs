namespace ESkitNet.Core.Entities;

public class ShoppingCart
{
    public required string Id { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = [];
}
