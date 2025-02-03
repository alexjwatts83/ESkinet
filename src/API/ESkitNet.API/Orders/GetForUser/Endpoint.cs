using ESkitNet.API.Orders.Dtos;
using ESkitNet.Core.Extensions;
using ESkitNet.Core.Specifications;

namespace ESkitNet.API.Orders.GetForUser;

public static class Endpoint
{
    public record Query() : IQuery<Result>;

    public record Result(IReadOnlyList<DisplayOrderDto> Orders);
    public record Response(IReadOnlyList<DisplayOrderDto> Orders);

    public static async Task<IResult> Handle(ISender sender)
    {
        var result = await sender.Send(new Query());

        var response = result.Adapt<Response>();

        return Results.Ok(response.Orders);
    }

    public class Handler(IHttpContextAccessor context, IUnitOfWork unitOfWork) : IQueryHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
        {
            var claimsPrincipal = context.HttpContext!.User;
            var email = claimsPrincipal.GetEmail(true);
            if (string.IsNullOrWhiteSpace(email))
                throw new BadHttpRequestException("Email not found for user");

            var specParams = new OrdersSpecParams(email);
            var spec = new OrderSpecification(specParams);

            var orders = await unitOfWork.Repository<Order, OrderId>().GetAllWithSpecAsync(spec, cancellationToken);

            if (orders.Any())
            {
                var orderDtos = orders.Adapt<IReadOnlyList<DisplayOrderDto>>();

                return new Result(orderDtos);
            }

            return new Result([]);
        }
    }
}
