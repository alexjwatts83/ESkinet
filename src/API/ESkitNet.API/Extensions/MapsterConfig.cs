using ESkitNet.API.Orders.Dtos;
using ESkitNet.API.Payments.Dtos;
using Mapster;
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

        TypeAdapterConfig<OrderItem, DisplayOrderItemDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.OrderId, src => src.OrderId.Value);

        TypeAdapterConfig<Order, DisplayOrderDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.Value);
            //.Map(dest => dest.DeliveryMethod, src => new DeliveryMethodDto(
            //    src.DeliveryMethod.Id.Value, src.DeliveryMethod.ShortName,
            //    src.DeliveryMethod.DeliveryTime, src.DeliveryMethod.Description,
            //    src.DeliveryMethod.Price));
            //.Map(dest => dest.OrderId, src => src.OrderId.Value);

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}
