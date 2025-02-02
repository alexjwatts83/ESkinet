using ESkitNet.Identity.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            context.Database.MigrateAsync().GetAwaiter().GetResult();

            await SeedAsync(context, userManager);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    private static async Task SeedAsync(StoreDbContext context, UserManager<AppUser> userManager)
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        await SeedDataAsync<Product, ProductId>(context, path!, "products");
        await SeedDataAsync<DeliveryMethod, DeliveryMethodId>(context, path!, "delivery");

        await SeedUsers(userManager);
    }

    private static async Task SeedUsers(UserManager<AppUser> userManager)
    {
        await SeedUser(userManager, "admin@test.com", "Admin", "User", "Admin");
        await SeedUser(userManager, "ckent@test.com", "Clark", "Kent", "Customer");
    }

    private static async Task SeedUser(UserManager<AppUser> userManager, string email, string firstName, string lastName, string role)
    {
        if (userManager.Users.Any(x => x.UserName == email))
            return;

        var user = new AppUser
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = email
        };

        await userManager.CreateAsync(user, "Pa$$wOrd321");
        await userManager.AddToRoleAsync(user, role);
    }

    private static async Task SeedDataAsync<TEntity, TKey>(StoreDbContext context, string dirPath, string jsonFileName)
        where TEntity : Entity<TKey>
    {
        if (await context.Set<TEntity>().AnyAsync())
            return;

        var path = dirPath + @$"/Data/SeedData/{jsonFileName}.json";

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
