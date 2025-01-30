namespace ESkitNet.Core.ValueObjects;

public record ProductId
{
    public Guid Value { get; }
    private ProductId(Guid value) => Value = value;

    public static ProductId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty)
            throw new DomainException($"{nameof(ProductId)} cannot be empty.");

        return new ProductId(value);
    }

    public static ProductId Of(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{nameof(ProductId)} cannot be empty.");

        var guid = new Guid(value);

        if (guid == Guid.Empty)
            throw new DomainException($"{nameof(ProductId)} cannot be empty.");

        return new ProductId(guid);
    }
}
