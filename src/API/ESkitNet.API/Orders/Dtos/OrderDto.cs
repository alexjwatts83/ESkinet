namespace ESkitNet.API.Orders.Dtos;

public record OrderDto
(
    string CartId,
    Guid DeliveryMethodId,
    ShippingAddressDto ShippingAddress,
    PaymentSummaryDto PaymentSummary
);
public record PaymentSummaryDto
(
    int Last4,
    string Brand,
    int ExpMonth,
    int ExpYear
);
public record ShippingAddressDto
(
    string Name,
    string Line1,
    string? Line2,
    string City,
    string State,
    string PostalCode,
    string Country
);
public record ProductItemOrderedDto
(
    Guid ProductId,
    string ProductName,
    string PictureUrl,
    string Type,
    string Brand
);

public record OrderItemDto(Guid OrderId, Guid ProductId, decimal Price, int Quantity);