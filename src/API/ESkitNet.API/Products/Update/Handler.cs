namespace ESkitNet.API.Products.Update;

public record Command(ProductDto Product) : ICommand<Result>;

public record Result(bool IsSuccess);

public class Handler(StoreDbContext dbContext): ICommandHandler<Command, Result>
{
    public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
    {
        //Update Order entity from command object
        //save to database
        //return result
        var productId = ProductId.Of(command.Product.Id);
        var product = await dbContext.Products
            .FindAsync([productId], cancellationToken: cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(command.Product.Id);

        //order.UpdateFromDto(command.Order);

        product.Name = command.Product.Name;
        product.Description = command.Product.Description;
        product.Price = command.Product.Price;
        product.PictureUrl = command.Product.PictureUrl;
        product.Type = command.Product.Type;
        product.ProductBrand = command.Product.ProductBrand;
        product.QuantityInStock = command.Product.QuantityInStock;

        dbContext.Products.Update(product);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new Result(true);
    }
}
