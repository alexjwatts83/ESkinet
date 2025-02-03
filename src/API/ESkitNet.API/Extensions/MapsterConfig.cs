using ESkitNet.API.Orders.Dtos;
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

        TypeAdapterConfig<Product, ProductDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.Value);

        TypeAdapterConfig<OrderItem, DisplayOrderItemDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.OrderId, src => src.OrderId.Value);

        TypeAdapterConfig<Order, DisplayOrderDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.DeliveryMethod, src => src.DeliveryMethod.Description)
            .Map(dest => dest.DeliveryMethodPrice, src => src.DeliveryMethod.Price)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.Total, src => src.Total());

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}
