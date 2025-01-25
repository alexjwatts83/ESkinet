namespace ESkitNet.API.Products.Dtos;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    string Type,
    string Brand,
    int QuantityInStock
);
