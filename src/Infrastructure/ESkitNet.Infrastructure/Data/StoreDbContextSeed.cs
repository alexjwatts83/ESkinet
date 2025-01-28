﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace ESkitNet.Infrastructure.Data;
public static class StoreDbContextSeedExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

            context.Database.MigrateAsync().GetAwaiter().GetResult();

            await SeedAsync(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    private static async Task SeedAsync(StoreDbContext context)
    {
        await SeedDataAsync<Product, ProductId>(context, "products");
        await SeedDataAsync<DeliveryMethod, DeliveryMethodId>(context, "delivery");
    }

    private static async Task SeedDataAsync<TEntity, TKey>(StoreDbContext context, string jsonFileName)
        where TEntity : Entity<TKey>
    {
        if (await context.Set<TEntity>().AnyAsync())
            return;

        var path = $"../../Infrastructure/ESkitNet.Infrastructure/Data/SeedData/{jsonFileName}.json";

        var exists = File.Exists(path);

        if (!exists)
            return;

        var entitiesRaw = await File.ReadAllTextAsync(path);
        var entities = JsonSerializer.Deserialize<List<TEntity>>(entitiesRaw);

        if (entities == null || entities.Count == 0)
            return;

        context.Set<TEntity>().AddRange(entities);

        await context.SaveChangesAsync();
    }
}
