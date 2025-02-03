using ESkitNet.API.Helpers;
using ESkitNet.API.Orders.Dtos;
using ESkitNet.Core.Specifications;

namespace ESkitNet.API.Admin.GetOrders;

public static class Endpoint
{
    public record Query(OrdersPaginationRequest PaginationRequest) : IQuery<Result>;

    public record Result(PaginatedResult<DisplayOrderDto> Orders);

    public class Handler(IUnitOfWork unitOfWork) : IQueryHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
        {
            var specParams = new OrdersPagingSpecParams(query.PaginationRequest);
            var spec = new OrderSpecification(specParams);
            var paginatedResult = await PaginatedResultHelpers.GetPageDetails<DisplayOrderDto, Order, OrderId>(
                unitOfWork, spec, specParams.PageNumber, specParams.PageSize, cancellationToken
            );

            return new Result(paginatedResult);
        }
    }

    public record Response(PaginatedResult<DisplayOrderDto> Orders);

    public static async Task<IResult> Handle([AsParameters] OrdersPaginationRequest request, ISender sender)
    {
        var result = await sender.Send(new Query(request));

        var response = result.Adapt<Response>();

        return Results.Ok(response.Orders);
    }
}
