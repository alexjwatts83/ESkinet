using ESkitNet.API.Extensions;
using ESkitNet.API.Services;
using ESkitNet.API.SignalR;
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
        services.AddHttpContextAccessor();

        services.AddCarter();

        services.AddControllers();

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

        services.AddSignalR();

        services.AddScoped<IStripeWebhookService, StripeWebhookService>();

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

        app.UseAuthentication();

        app.UseAuthorization();

        //if (!app.Environment.IsDevelopment())
        //{
        //    app.UseDefaultFiles();

        //    app.UseStaticFiles();
        //}

        app.UseDefaultFiles();

        app.UseStaticFiles();

        app.MapCarter();

        // for fall back controller
        app.MapControllers();

        app.MapHub<NotificationHub>("/hub/notifications");

        //if (!app.Environment.IsDevelopment())
        //    app.MapFallbackToController("Index", "Fallback");

        //app.MapFallbackToController("Index", "Fallback");
        app.MapFallbackToFile("Index.html");

        return app;
    }
}
