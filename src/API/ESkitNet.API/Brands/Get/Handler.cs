using ESkitNet.Core.Specifications;

namespace ESkitNet.API.Brands.Get;

public record Query() : IQuery<Result>;

public record Result(IReadOnlyList<string> Brands);

public class Handler(IGenericRepository<Product, ProductId> repo) : IQueryHandler<Query, Result>
{
    public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
    {
        var spec = new BrandListSpecification();
        var brands = await repo.GetAllWithSpecAsync(spec, cancellationToken);

        return new Result(brands);
    }
}
