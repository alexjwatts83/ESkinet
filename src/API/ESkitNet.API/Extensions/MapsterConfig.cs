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

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}
