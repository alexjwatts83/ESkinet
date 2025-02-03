using ESkitNet.API.Helpers;
using ESkitNet.Core.Specifications;

namespace ESkitNet.API.Products.Get;

public static class Endpoint
{
    public record Query(ProductsPaginationRequest PaginationRequest) : IQuery<Result>;

    public record Result(PaginatedResult<ProductDto> Products);

    public class Handler(IUnitOfWork unitOfWork) : IQueryHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
        {
            // TODO: return the dto by adding some additional mapper or something like tha
            //       so the mapping isn't done here
            var specParams = new ProductSpecParams(query.PaginationRequest);
            var spec = new ProductSpecification(specParams);
            var paginatedResult = await PaginatedResultHelpers.GetPageDetails<ProductDto, Product, ProductId>(
                unitOfWork, spec, specParams.PageNumber, specParams.PageSize, cancellationToken
            );

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
