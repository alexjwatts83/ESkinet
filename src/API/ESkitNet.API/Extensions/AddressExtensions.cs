using ESkitNet.API.Accounts.Dtos;

namespace ESkitNet.API.Extensions;

public static class AddressExtensions
{
    public static void UpdateFromDto(this Address address, AddressDto dto)
    {
        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(dto);

        address.Line1 = dto.Line1;
        address.Line2 = dto.Line2;
        address.City = dto.City;
        address.State = dto.State;
        address.PostalCode = dto.PostalCode;
        address.Country = dto.Country;
    }
}
