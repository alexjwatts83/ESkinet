namespace ESkitNet.API.Products.Delete;

public record Command(Guid ProductId) : ICommand<Result>;

public record Result(bool IsSuccess);

public class Handler(StoreDbContext dbContext) : ICommandHandler<Command, Result>
{
    public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
    {
        //Delete Order entity from command object
        //save to database
        //return result

        var productId = ProductId.Of(command.ProductId);
        var product = await dbContext.Products
            .FindAsync([productId], cancellationToken: cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(command.ProductId);

        dbContext.Products.Remove(product);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new Result(true);
    }
}
