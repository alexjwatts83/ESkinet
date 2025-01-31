using ESkitNet.API.Extensions;
using ESkitNet.API.Payments.StripeWebhook;
using ESkitNet.Core.Behaviors;
using ESkitNet.Core.Exceptions.Handler;
using ESkitNet.Identity.Entities;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Reflection;

namespace ESkitNet.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IStripeWebhookService, StripeWebhookService>();

        services.AddHttpContextAccessor();

        services.AddCarter();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        // Fluent Validation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("Database")!)
            .AddRedis(configuration.GetConnectionString("Redis")!);
        
        services.AddAuthorization();
        services
            .AddIdentityApiEndpoints<AppUser>()
            .AddEntityFrameworkStores<StoreDbContext>();

        services.RegisterMapsterConfiguration();

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {

        // TODO delete ExceptionMiddleware altogether later on
        //app.UseMiddleware<ExceptionMiddleware>();
        app.UseExceptionHandler(options => { });

        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        app.MapCarter();

        //app.MapIdentityApi<AppUser>();

        //app.MapGroup("/account").MapIdentityApi<AppUser>();

        return app;
    }
}
