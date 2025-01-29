using ESkitNet.API.Payments.Dtos;

namespace ESkitNet.API.Payments.GetMethods;

public static class Endpoint
{
    public static async Task<IResult> Handle(IGenericRepository<DeliveryMethod, DeliveryMethodId> repository, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllAsync(cancellationToken);

        var sorted = result
            .Select(x => x.Adapt<DeliveryMethodDto>())
            .OrderBy(x => x.Price);

        return Results.Ok(sorted);
    }
}
