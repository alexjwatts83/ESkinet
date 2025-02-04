using System.Text;
using System.Text.Json;

namespace ESkitNet.API.EndpointFilters;

public class CacheFilterMetadata(int timeToLiveSeconds)
{
    public int TimeToLiveSeconds { get; set; } = timeToLiveSeconds;
}

// TODO I really dont like this but it works
public class CacheFilter<TEntity> : IEndpointFilter
    where TEntity : class
{
    public virtual async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

        var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

        var cachedResponse = await cacheService.GetCacheResponseAsync(cacheKey);

        if (string.IsNullOrWhiteSpace(cachedResponse))
        {
            var result = await next(context);

            if (result is Microsoft.AspNetCore.Http.HttpResults.Ok<PaginatedResult<TEntity>> okPagingResult)
            {
                if (okPagingResult.Value != null)
                {
                    var timeToLiveSeconds = 2 * 60;
                    if (context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<CacheFilterMetadata>() is { } meta)
                    {
                        timeToLiveSeconds = meta.TimeToLiveSeconds;
                    }
                    await cacheService.CacheResponseAsync(cacheKey, okPagingResult.Value, TimeSpan.FromSeconds(timeToLiveSeconds));
                }
            }
            return result;
        }

        // cant deserialise for some reason which is why i use the type object
        //var deserialised = JsonSerializer.Deserialize<PaginatedResult<TEntity>>(cachedResponse);
        var deserialised = JsonSerializer.Deserialize<object>(cachedResponse);

        return Results.Json(deserialised);
    }

    private static string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();

        keyBuilder.Append($"{request.Path}");

        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
        {
            keyBuilder.Append($"|{key}-{value}");
        }

        return keyBuilder.ToString();
    }
}