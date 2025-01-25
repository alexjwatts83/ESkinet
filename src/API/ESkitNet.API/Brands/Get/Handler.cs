namespace ESkitNet.API.Brands.Get;

public record Query() : IQuery<Result>;

public record Result(IReadOnlyList<string> Brands);

public class Handler(IProductRepository productRepository) : IQueryHandler<Query, Result>
{
    public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
    {
        var brands = await productRepository.GetBrandsAsync(cancellationToken);

        return new Result(brands);
    }
}
