namespace ESkitNet.API.Products.Delete;

public record Command(Guid ProductId) : ICommand<Result>;

public record Result(bool IsSuccess);

public class Handler(IProductRepository productRepository) : ICommandHandler<Command, Result>
{
    public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(command.ProductId);

        productRepository.Delete(product);

        var result = await productRepository.SaveChangesAsync(cancellationToken);

        return new Result(result);
    }
}
