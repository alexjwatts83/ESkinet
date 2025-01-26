using ESkitNet.Core.Specifications;

namespace ESkitNet.API.Types.Get;

public record Query(string? Sort) : IQuery<Result>;

public record Result(IReadOnlyList<string> Types);

public class Handler(IGenericRepository<Product, ProductId> repo) : IQueryHandler<Query, Result>
{
    public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
    {
        var spec = new TypeListSpecification(query.Sort);
        var brands = await repo.GetAllWithSpecAsync(spec, cancellationToken);

        return new Result(brands);
    }
}
