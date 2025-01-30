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

    public static DeliveryMethodId Of(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{nameof(DeliveryMethodId)} cannot be empty.");

        var guid = new Guid(value);

        if (guid == Guid.Empty)
            throw new DomainException($"{nameof(DeliveryMethodId)} cannot be empty.");

        return new DeliveryMethodId(guid);
    }
}