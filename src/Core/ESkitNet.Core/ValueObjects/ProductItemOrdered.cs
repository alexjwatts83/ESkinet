namespace ESkitNet.Core.ValueObjects;

public record ProductItemOrdered
{
    public required Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string PictureUrl { get; set; }
    public required string Type { get; set; }
    public required string Brand { get; set; }
}
