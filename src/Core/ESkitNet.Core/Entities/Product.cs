namespace ESkitNet.Core.Entities;

public class Product : Entity<ProductId>
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required string PictureUrl { get; set; }
    public required string Type { get; set; }
    public required string ProductBrand { get; set; }
    public int QuantityInStock { get; set; }
}
