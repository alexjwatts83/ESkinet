using System.Reflection;

namespace ESkitNet.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        //services.AddExceptionHandler<CustomExceptionHandler>();

        //services.AddHealthChecks()
        //    .AddSqlServer(configuration.GetConnectionString("Database")!);

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();

        //app.UseExceptionHandler(options => { });

        //app.UseHealthChecks("/health",
        //    new HealthCheckOptions
        //    {
        //        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        //    });

        return app;
    }
}
