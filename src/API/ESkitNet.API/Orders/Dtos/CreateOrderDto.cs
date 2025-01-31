using ESkitNet.API.Payments.Dtos;

namespace ESkitNet.API.Orders.Dtos;

public record CreateOrderDto
(
    string CartId,
    Guid DeliveryMethodId,
    ShippingAddressDto ShippingAddress,
    PaymentSummaryDto PaymentSummary
);

public record DisplayOrderDto
(
    Guid Id,
    DateTime OrderDate,
    string BuyerEmail,
    string DeliveryMethod,
    decimal DeliveryMethodPrice,
    ShippingAddressDto ShippingAddress,
    PaymentSummaryDto PaymentSummary,
    string Status,
    decimal SubTotal,
    string PaymentIntentId,
    List<DisplayOrderItemDto> OrderItems,
    decimal Total
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

public record DisplayOrderItemDto(
    Guid Id,
    Guid OrderId,
    ProductItemOrderedDto ItemOrdered,
    decimal Price, 
    int Quantity
);