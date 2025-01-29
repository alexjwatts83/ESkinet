using ESkitNet.API.Payments.Dtos;

namespace ESkitNet.API.Payments.GetMethods;

public static class Endpoint
{
    public static async Task<IResult> Handle(IGenericRepository<DeliveryMethod, DeliveryMethodId> repository, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllAsync(cancellationToken);

        return Results.Ok(result.Select(x => x.Adapt<DeliveryMethodDto>()));
    }
}
