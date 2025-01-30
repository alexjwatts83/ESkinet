namespace ESkitNet.Core.ValueObjects;

public record OrderId
{
    public Guid Value { get; }
    private OrderId(Guid value) => Value = value;

    public static OrderId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty)
            throw new DomainException($"{nameof(OrderId)} cannot be empty.");

        return new OrderId(value);
    }

    public static OrderId Of(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{nameof(OrderId)} cannot be empty.");

        var guid = new Guid(value);

        if (guid == Guid.Empty)
            throw new DomainException($"{nameof(OrderId)} cannot be empty.");

        return new OrderId(guid);
    }
}
