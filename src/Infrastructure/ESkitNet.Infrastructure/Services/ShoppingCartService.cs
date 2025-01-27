using ESkitNet.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ESkitNet.Infrastructure.Services;

public class ShoppingCartService(IDistributedCache cache, ILogger<ShoppingCartService> logger) : IShoppingCartService
{
    public async Task<bool> DeleteAsync(string id)
    {
        logger.LogInformation("Delete Cart of {Id}", id);

        await cache.RemoveAsync(id);

        return true;
    }

    public async Task<ShoppingCart?> GetAsync(string id)
    {
        logger.LogInformation("Get Cart of {Id}", id);

        var cached = await cache.GetStringAsync(id);

        if (!string.IsNullOrEmpty(cached))
            return JsonSerializer.Deserialize<ShoppingCart>(cached)!;

        var newCart = new ShoppingCart() { Id = id };

        return newCart;
    }

    public async Task<ShoppingCart?> SetAsync(ShoppingCart cart)
    {
        logger.LogInformation("Set Cart of {Cart}", cart);

        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
        };

        await cache.SetStringAsync(cart.Id, JsonSerializer.Serialize(cart), options);

        return await GetAsync(cart.Id);
    }
}
