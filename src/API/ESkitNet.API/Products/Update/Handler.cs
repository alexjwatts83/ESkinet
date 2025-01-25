namespace ESkitNet.API.Products.Update;

public record Command(ProductDto Product) : ICommand<Result>;

public record Result(bool IsSuccess);

public class Handler(IProductRepository productRepository) : ICommandHandler<Command, Result>
{
    public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(command.Product.Id, cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(command.Product.Id);

        // TODO: use a mapper
        product.Name = command.Product.Name;
        product.Description = command.Product.Description;
        product.Price = command.Product.Price;
        product.PictureUrl = command.Product.PictureUrl;
        product.Type = command.Product.Type;
        product.ProductBrand = command.Product.ProductBrand;
        product.QuantityInStock = command.Product.QuantityInStock;

        productRepository.Update(product);

        var result = await productRepository.SaveChangesAsync(cancellationToken);

        return new Result(result);
    }
}
