namespace ESkitNet.Core.Entities;

public class ShoppingCartItem
{
    public required string ProductId { get; set; }
    public required string ProductName { get; set; }
    public decimal Price { get; set; }
    public required string PictureUrl { get; set; }
    public required string Type { get; set; }
    public required string Brand { get; set; }
    public int Quantity { get; set; }
}
