using ESkitNet.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ESkitNet.Infrastructure.Services;

public class ShoppingCartService(/*IConnectionMultiplexer redis,*/ IDistributedCache cache, ILogger<ShoppingCartService> logger) : IShoppingCartService
{
    //private readonly IDatabase _database = redis.GetDatabase();
    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Delete Cart of {Id}", id);

        await cache.RemoveAsync(id, cancellationToken);

        return true;

        //return await _database.KeyDeleteAsync(id);
    }

    public async Task<ShoppingCart?> GetAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Get Cart of {Id}", id);

        //var cached = await _database.StringGetAsync(id);

        //return cached.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(cached!);

        var cached = await cache.GetStringAsync(id, cancellationToken);

        if (!string.IsNullOrEmpty(cached))
            return JsonSerializer.Deserialize<ShoppingCart>(cached)!;

        return null;

        //var newCart = new ShoppingCart() { Id = id };

        //await SetAsync(cache, newCart, cancellationToken);

        //return newCart;
    }

    public async Task<ShoppingCart?> SetAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        logger.LogInformation("Set Cart of {Cart}", cart);

        //var created = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(5));

        //if (!created)
        //    return null;

        await SetAsync(cache, cart, cancellationToken);

        return await GetAsync(cart.Id, cancellationToken);
    }

    private static async Task SetAsync(IDistributedCache cache, ShoppingCart cart, CancellationToken cancellationToken)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
        };

        await cache.SetStringAsync(cart.Id, JsonSerializer.Serialize(cart), options, cancellationToken);
    }
}
