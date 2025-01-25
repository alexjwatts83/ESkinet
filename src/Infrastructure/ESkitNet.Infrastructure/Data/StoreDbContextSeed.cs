using Microsoft.AspNetCore.Builder;
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
        await SeedProductAsync(context);
    }

    private static async Task SeedProductAsync(StoreDbContext context)
    {
        if (await context.Products.AnyAsync())
            return;

        var path = "../../Infrastructure/ESkitNet.Infrastructure/Data/SeedData/products.json";

        var exists = File.Exists(path);

        if (!exists)
            return;

        var productData = await File.ReadAllTextAsync(path);
        var products = JsonSerializer.Deserialize<List<Product>>(productData);

        if (products == null || products.Count == 0) 
            return;

        context.Products.AddRange(products);

        await context.SaveChangesAsync();
    }
}
