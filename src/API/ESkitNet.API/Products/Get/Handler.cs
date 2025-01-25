namespace ESkitNet.API.Products.Get;

public record Query(PaginationRequest PaginationRequest) : IQuery<Result>;

public record Result(PaginatedResult<ProductDto> Products);

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

        var products = await dbContext.Products
                       .OrderBy(o => o.Name)
                       .Skip(pageSize * (pageNumber - 1))
                       .Take(pageSize)
                       .ToListAsync(cancellationToken);
        var productDtos = products
            .Select(x => new ProductDto(x.Id.Value, x.Name, x.Description, x.Price, x.PictureUrl, x.Type, x.ProductBrand, x.QuantityInStock));
        var result = new PaginatedResult<ProductDto>(
            pageNumber,
            pageSize,
            totalCount,
            productDtos
        );
        return new Result(result);
    }
}
