namespace ESkitNet.Core.ValueObjects;

public record AddressId
{
    public Guid Value { get; }
    private AddressId(Guid value) => Value = value;

    public static AddressId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty)
            throw new DomainException($"{nameof(AddressId)} cannot be empty.");

        return new AddressId(value);
    }
}