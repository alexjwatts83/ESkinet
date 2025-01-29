namespace ESkitNet.API.Payments.Dtos;

public record DeliveryMethodDto(
    Guid Id,
    string ShortName,
    string DeliveryTime,
    string Description,
    decimal Price
);
