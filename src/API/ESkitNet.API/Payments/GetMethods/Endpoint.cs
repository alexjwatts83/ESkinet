using ESkitNet.API.Payments.Dtos;

namespace ESkitNet.API.Payments.GetMethods;

public static class Endpoint
{
    public static async Task<IResult> Handle(IUnitOfWork unitOfWork, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Repository<DeliveryMethod, DeliveryMethodId>().GetAllAsync(cancellationToken);

        var sorted = result
            .Select(x => x.Adapt<DeliveryMethodDto>())
            .OrderBy(x => x.Price);

        return Results.Ok(sorted);
    }
}
