namespace ESkitNet.API.Products.Delete;

public record Command(Guid ProductId) : ICommand<Result>;

public record Result(bool IsSuccess);

public class Handler(IGenericRepository<Product, ProductId> repo) : ICommandHandler<Command, Result>
{
    public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
    {
        var product = await repo.GetByIdAsync(ProductId.Of(command.ProductId), cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(command.ProductId);

        repo.Delete(product);

        var result = await repo.SaveAllAsync(cancellationToken);

        return new Result(result);
    }
}
