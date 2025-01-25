﻿namespace ESkitNet.API.Products.Create;

public record Command(ProductDto Product) : ICommand<Result>;
public record Result(Guid Id);

public class Handler(IProductRepository productRepository) : ICommandHandler<Command, Result>
{
    public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
    {
        // TODO use a mapper
        var product = new Product() 
        { 
            Id = ProductId.Of(Guid.NewGuid()),
            Name = command.Product.Name,
            Description = command.Product.Description,
            Price = command.Product.Price,
            PictureUrl = command.Product.PictureUrl,
            Type = command.Product.Type,
            ProductBrand = command.Product.ProductBrand,
            QuantityInStock = command.Product.QuantityInStock,
        };

        productRepository.Add(product);

        await productRepository.SaveChangesAsync(cancellationToken);

        return new Result(product.Id.Value);
    }
}
