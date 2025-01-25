namespace ESkitNet.API.Types.Get;

public record Query() : IQuery<Result>;

public record Result(IReadOnlyList<string> Types);

public class Handler(IProductRepository productRepository) : IQueryHandler<Query, Result>
{
    public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
    {
        var brands = await productRepository.GetTypesAsync(cancellationToken);

        return new Result(brands);
    }
}
