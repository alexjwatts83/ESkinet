namespace ESkitNet.Core.ValueObjects;

public record DeliveryMethodId
{
    public Guid Value { get; }
    private DeliveryMethodId(Guid value) => Value = value;

    public static DeliveryMethodId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty)
            throw new DomainException($"{nameof(DeliveryMethodId)} cannot be empty.");

        return new DeliveryMethodId(value);
    }
}