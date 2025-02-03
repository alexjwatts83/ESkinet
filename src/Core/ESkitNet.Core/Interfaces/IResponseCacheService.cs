namespace ESkitNet.Core.Interfaces;

public interface IResponseCacheService
{
    Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive, CancellationToken cancellationToken = default);
    Task<string?> GetCacheResponseAsync(string cacheKey, CancellationToken cancellationToken = default);

    Task RemoveCachePattern(string pattern, CancellationToken cancellationToken = default);
}
