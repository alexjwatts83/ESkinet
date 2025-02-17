﻿using ESkitNet.API.Orders.Dtos;
using ESkitNet.Core.Extensions;
using ESkitNet.Core.Specifications;

namespace ESkitNet.API.Orders.GetForUserById;

public static class Endpoint
{
    public record Query(Guid Id) : IQuery<Result>;

    public record Result(DisplayOrderDto Order);
    public record Response(DisplayOrderDto Order);

    public static async Task<IResult> Handle(Guid id, ISender sender)
    {
        var result = await sender.Send(new Query(id));

        var response = result.Adapt<Response>();

        return Results.Ok(response.Order);
    }

    public class Handler(IHttpContextAccessor context, IUnitOfWork unitOfWork) : IQueryHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
        {
            var claimsPrincipal = context.HttpContext!.User;
            var email = claimsPrincipal.GetEmail(true);
            if (string.IsNullOrWhiteSpace(email))
                throw new BadHttpRequestException("Email not found for user");

            var request = new OrderRequest(email, query.Id);
            var specParams = new OrderSpecParams(request);
            var spec = new OrderSpecification(specParams);

            var order = await unitOfWork.Repository<Order, OrderId>().GetOneWithSpecAsync(spec, cancellationToken);

            if (order == null)
                throw new OrderNotFoundException(query.Id);

            return new Result(order.Adapt<DisplayOrderDto>());
        }
    }
}
