using ESkitNet.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ESkitNet.Infrastructure.Services;

public class ShoppingCartService(IDistributedCache cache, ILogger<ShoppingCartService> logger) : IShoppingCartService
{
    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Delete Cart of {Id}", id);

        await cache.RemoveAsync(id, cancellationToken);

        return true;
    }

    public async Task<ShoppingCart?> GetAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Get Cart of {Id}", id);

        var cached = await cache.GetStringAsync(id, cancellationToken);

        if (!string.IsNullOrEmpty(cached))
            return JsonSerializer.Deserialize<ShoppingCart>(cached)!;

        var newCart = new ShoppingCart() { Id = id };

        await SetAsync(cache, newCart, cancellationToken);

        return newCart;
    }

    public async Task<ShoppingCart?> SetAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        logger.LogInformation("Set Cart of {Cart}", cart);

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
