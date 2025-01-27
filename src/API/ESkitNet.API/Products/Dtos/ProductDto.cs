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

//public record ShoppingCartDto(
//    string Id,
//    List<ShoppingCartItemDto> Items
//);

//public record ShoppingCartItemDto(
//    string ProductId,
//    string ProductName,
//    decimal Price,
//    string PictureUrl,
//    string Type,
//    string Brand,
//    int Quantity
//);
