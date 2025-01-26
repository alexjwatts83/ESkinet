using ESkitNet.Core.Specifications;

namespace ESkitNet.API.Products.Get;

public record Query(ProductsPaginationRequest PaginationRequest) : IQuery<Result>;

public record Result(PaginatedResult<ProductDto> Products);

public class Handler(IGenericRepository<Product, ProductId> repo) : IQueryHandler<Query, Result>
{
    public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
    {
        //// TODO: return the dto by adding some additional mapper or something like tha
        ////       so the mapping isn't done here
        //var (pageNumber, pageSize, count, products) = await productRepository.GetAsync(query.PaginationRequest, cancellationToken);

        //var productDtos = products
        //    .Select(x => new ProductDto(x.Id.Value, x.Name, x.Description, x.Price, x.PictureUrl, x.Type, x.Brand, x.QuantityInStock));
        var spec = new ProductSpecification(query.PaginationRequest.Brand, query.PaginationRequest.Type, query.PaginationRequest.Sort);
        var products = await repo.ListAllWithSpecificationAsync(spec, cancellationToken);
        var productDtos = products
            .Select(x => new ProductDto(x.Id.Value, x.Name, x.Description, x.Price, x.PictureUrl, x.Type, x.Brand, x.QuantityInStock));
        var paginatedResult = new PaginatedResult<ProductDto>(1, products.Count, products.Count, productDtos);

        return new Result(paginatedResult);
    }
}
