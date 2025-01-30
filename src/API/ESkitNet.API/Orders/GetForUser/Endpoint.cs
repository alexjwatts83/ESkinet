using ESkitNet.Core.Extensions;
using ESkitNet.Core.Specifications;

namespace ESkitNet.API.Orders.GetForUser;

public static class Endpoint
{
    public record Query() : IQuery<Result>;

    public record Result(IReadOnlyList<Order> Orders);
    public record Response(IReadOnlyList<Order> Orders);

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

            var spec = new OrderSpecification(email);

            var orders = await unitOfWork.Repository<Order, OrderId>().GetAllWithSpecAsync(spec, cancellationToken);

            if (orders.Any())
            {
                //var orderDtos = orders.Select(x =>
                //{
                //    var displayOrderDto = x.Adapt<DisplayOrderDto>();

                //    return displayOrderDto;

                //}).ToList().AsReadOnly();
                // TODO figure out how to map fields ShippingAddress, DeliveryMethod, PaymentSummary

                return new Result(orders);
            }

            return new Result([]);
        }
    }
}
