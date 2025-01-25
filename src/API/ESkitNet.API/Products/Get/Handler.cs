namespace ESkitNet.API.Products.Get;

public record Query(PaginationRequest PaginationRequest) : IQuery<Result>;

public record Result(PaginatedResult<ProductDto> Products);

public class Handler(IProductRepository productRepository) : IQueryHandler<Query, Result>
{
    public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
    {
        // TODO: return the dto by adding some additional mapper or something like tha
        //       so the mapping isn't done here
        var (pageNumber, pageSize, count, products) = await productRepository.GetAsync(query.PaginationRequest, cancellationToken);

        var productDtos = products
            .Select(x => new ProductDto(x.Id.Value, x.Name, x.Description, x.Price, x.PictureUrl, x.Type, x.ProductBrand, x.QuantityInStock));

        var paginatedResult = new PaginatedResult<ProductDto>(pageNumber, pageSize, count, productDtos);

        return new Result(paginatedResult);
    }
}
