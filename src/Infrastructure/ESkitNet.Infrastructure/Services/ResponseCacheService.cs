using ESkitNet.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ESkitNet.Infrastructure.Services;

public class ResponseCacheService : IResponseCacheService
{
    private List<string> Keys = [];
    private readonly IDistributedCache cache;
    private readonly string _cacheKeysKey = "_cache-keys_";

    public ResponseCacheService(IDistributedCache cache)
    {
        this.cache = cache;
        //Task.Run(async () => await this.Init());
        //Init();
    }

    private async Task Init(CancellationToken cancellationToken = default)
    {
        if (Keys.Count > 0)
            return;

        //var data = await GetCacheResponseAsync(_cacheKeysKey);
        var data = await cache.GetStringAsync(_cacheKeysKey, cancellationToken);
        if (string.IsNullOrEmpty(data))
            return;

        var keys = JsonSerializer.Deserialize<List<string>>(data);

        if (keys == null) 
            return;

        Keys = keys;
    }

    private async Task CacheKeyAsync(string cacheKey, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default)
    {
        // TODO update how the time to live works based on the
        Keys.Add(cacheKey);

        var serialisedObject = JsonSerializer.Serialize(Keys, GetJsonSerializerOptions());

        await cache.SetStringAsync(_cacheKeysKey, serialisedObject, options, cancellationToken);
    }

    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive, CancellationToken cancellationToken = default)
    {
        await Init();

        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = timeToLive
        };

        var serialisedObject = JsonSerializer.Serialize(response, GetJsonSerializerOptions());

        await cache.SetStringAsync(cacheKey, serialisedObject, options, cancellationToken);

        await CacheKeyAsync(cacheKey, options, cancellationToken);
    }

    public async Task<string?> GetCacheResponseAsync(string cacheKey, CancellationToken cancellationToken = default)
    {
        await Init();

        var cached = await cache.GetStringAsync(cacheKey, cancellationToken);

        return cached;
    }

    public async Task RemoveCachePattern(string pattern, CancellationToken cancellationToken = default)
    {
        await Init();

        var keysByPattern = Keys.Where(x => x.StartsWith(pattern)).ToList();

        foreach (var key in keysByPattern) 
        { 
            await cache.RemoveAsync(key, cancellationToken);
        }
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
