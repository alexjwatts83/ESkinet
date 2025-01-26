using ESkitNet.Core.Specifications;

namespace ESkitNet.API.Products.Get;

public static class Endpoint
{
    public record Query(ProductsPaginationRequest PaginationRequest) : IQuery<Result>;

    public record Result(PaginatedResult<ProductDto> Products);

    public class Handler(IGenericRepository<Product, ProductId> repo) : IQueryHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
        {
            // TODO: return the dto by adding some additional mapper or something like tha
            //       so the mapping isn't done here
            var specParams = new ProductSpecParams(
                query.PaginationRequest.Brand,
                query.PaginationRequest.Type,
                query.PaginationRequest.Sort,
                query.PaginationRequest.Search,
                query.PaginationRequest.PageNumber,
                query.PaginationRequest.PageSize
            );
            var spec = new ProductSpecification(specParams);
            var products = await repo.GetAllWithSpecAsync(spec, cancellationToken);
            var count = await repo.CountAsync(spec, cancellationToken);
            var productDtos = products
                .Select(x => new ProductDto(x.Id.Value, x.Name, x.Description, x.Price, x.PictureUrl, x.Type, x.Brand, x.QuantityInStock));
            var paginatedResult = new PaginatedResult<ProductDto>(query.PaginationRequest.PageNumber, query.PaginationRequest.PageSize, count, productDtos);

            return new Result(paginatedResult);
        }
    }

    public record Response(PaginatedResult<ProductDto> Products);

    public static async Task<IResult> Handle([AsParameters] ProductsPaginationRequest request, ISender sender)
    {
        var result = await sender.Send(new Query(request));

        var response = result.Adapt<Response>();

        return Results.Ok(response.Products);
    }
}
