namespace ESkitNet.Core.ValueObjects;

public record OrderDeliveryMethod
{
    public required Guid DeliveryMethodId { get; set; }
    public required string ShortName { get; set; }
    public required string DeliveryTime { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
}