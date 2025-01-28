namespace ESkitNet.Core.Entities;

public class DeliveryMethod : Entity<DeliveryMethodId>
{
    public required string ShortName { get; set; }
    public required string DeliveryTime { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
}