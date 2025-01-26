namespace ESkitNet.API.Products.Update;

public static class Endpoint
{
    public record Command(ProductDto Product) : ICommand<Result>;

    public record Result(bool IsSuccess);

    public class Handler(IGenericRepository<Product, ProductId> repo) : ICommandHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var productId = ProductId.Of(command.Product.Id);
            var product = await repo.GetByIdAsync(productId, cancellationToken);

            if (product is null)
                throw new ProductNotFoundException(command.Product.Id);

            // TODO: use a mapper
            product.Name = command.Product.Name;
            product.Description = command.Product.Description;
            product.Price = command.Product.Price;
            product.PictureUrl = command.Product.PictureUrl;
            product.Type = command.Product.Type;
            product.Brand = command.Product.Brand;
            product.QuantityInStock = command.Product.QuantityInStock;

            repo.Update(product);

            var result = await repo.SaveAllAsync(cancellationToken);

            return new Result(result);
        }
    }
    public record Request(ProductDto Product);

    public record Response(bool IsSuccess);

    public static async Task<IResult> Handle(Guid id, Request request, ISender sender)
    {
        var command = request.Adapt<Command>();

        var result = await sender.Send(command);

        var response = result.Adapt<Response>();

        // TODO return better response
        return (response == null)
            ? Results.BadRequest("Failed to update Product")
            : Results.NoContent();
    }

}
