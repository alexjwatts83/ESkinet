namespace ESkitNet.API.Products.Get;

public record Query(PaginationRequest PaginationRequest) : IQuery<Result>;

public record Result(PaginatedResult<Product> Products);

public class Handler(StoreDbContext dbContext)
    : IQueryHandler<Query, Result>
{
    public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
    {
        // get orders with pagination
        // return result
        var pageNumber = query.PaginationRequest.PageNumber <= 0
            ? 1
            : query.PaginationRequest.PageNumber;
        var pageSize = query.PaginationRequest.PageSize;

        var totalCount = await dbContext.Products.LongCountAsync(cancellationToken);

        var orders = await dbContext.Products
                       .OrderBy(o => o.Name)
                       .Skip(pageSize * (pageNumber - 1))
                       .Take(pageSize)
                       .ToListAsync(cancellationToken);

        return new Result(
            new PaginatedResult<Product>(
                pageNumber,
                pageSize,
                totalCount,
                orders));
    }
}
