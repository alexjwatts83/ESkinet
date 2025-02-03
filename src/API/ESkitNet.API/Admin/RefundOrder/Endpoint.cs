using ESkitNet.API.Orders.Dtos;
using ESkitNet.Core.Specifications;

namespace ESkitNet.API.Admin.RefundOrder;

public static class Endpoint
{
    public record Request(Guid Id);
    public record Response(DisplayOrderDto Order);

    public static async Task<IResult> Handle(Request request, ISender sender)
    {
        var command = request.Adapt<Command>();

        var result = await sender.Send(command);

        var response = result.Adapt<Response>();

        return Results.Ok(response.Order);
    }

    public record Command(Guid Id) : ICommand<Result>;
    public record Result(DisplayOrderDto Order);

    public class Handler(IPaymentService paymentService, IUnitOfWork unitOfWork)
        : ICommandHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var request = new OrderRequest()
            {
                Id = command.Id,
            };
            var specParams = new OrderSpecParams(request);
            var spec = new OrderSpecification(specParams);
            var order = await unitOfWork.Repository<Order, OrderId>().GetOneWithSpecAsync(spec, cancellationToken);

            if (order == null)
                throw new OrderNotFoundException(command.Id);

            if (order.Status == OrderStatus.Pending)
                throw new BadHttpRequestException("No payment received for this order");

            if (order.Status == OrderStatus.Refunded)
                throw new BadHttpRequestException("Payment has already been refunded");

            var result = await paymentService.RefundPayment(order.PaymentIntentId);

            if (result == "succeeded")
            {
                order.Status = OrderStatus.Refunded;

                if (await unitOfWork.Complete(cancellationToken))
                    throw new BadHttpRequestException("Payment was refunded but an error occurred saving details to the database");

                return new Result(order.Adapt<DisplayOrderDto>());
            }

            throw new BadHttpRequestException($"Payment refund failed with status of '{result}'");
        }
    }
}
