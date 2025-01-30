using ESkitNet.API.Payments.Dtos;
using System.Reflection;

namespace ESkitNet.API.Extensions;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<DeliveryMethod, DeliveryMethodDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.Value);

        TypeAdapterConfig<Product, ProductItemOrdered>
            .NewConfig()
            .Map(dest => dest.ProductId, src => src.Id.Value)
            .Map(dest => dest.ProductName, src => src.Name);

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}
