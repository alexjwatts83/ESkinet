using ESkitNet.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ESkitNet.Infrastructure.Services;

public class ResponseCacheService(IDistributedCache cache) : IResponseCacheService
{
    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive, CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = timeToLive
        };

        var serialisedObject = JsonSerializer.Serialize(response, GetJsonSerializerOptions());

        await cache.SetStringAsync(cacheKey, serialisedObject, options, cancellationToken);
    }

    public async Task<string?> GetCacheResponseAsync(string cacheKey, CancellationToken cancellationToken = default)
    {
        var cached = await cache.GetStringAsync(cacheKey, cancellationToken);

        return cached;
    }

    public Task RemoveCachePattern(string pattern, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        return jsonOptions;
    }
}
