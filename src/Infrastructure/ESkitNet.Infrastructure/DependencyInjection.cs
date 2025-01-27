using ESkitNet.Core.Interfaces;
using ESkitNet.Infrastructure.Data.Interceptors;
using ESkitNet.Infrastructure.Data.Services;
using ESkitNet.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ESkitNet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        var connectionString = configuration.GetConnectionString("Database");

        // common services
        services.AddScoped<IAppTimeProvider, AppTimeProvider>();
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddScoped<IShoppingCartService, ShoppingCartService>();

        // db services
        // No <>
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<StoreDbContext>((sp, options) =>
        {
            var interceptors = sp.GetServices<ISaveChangesInterceptor>();

            options.AddInterceptors(interceptors);

            options.UseSqlServer(connectionString);
        });

        return services;
    }
}
