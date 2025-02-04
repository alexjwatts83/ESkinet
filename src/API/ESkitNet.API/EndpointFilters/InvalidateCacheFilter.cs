
namespace ESkitNet.API.EndpointFilters;

public class InvalidateCacheFilterMetadata(string pattern)
{
    public string Pattern { get; set; } = pattern;
}

public class InvalidateCacheFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);

        if (result is Microsoft.AspNetCore.Http.HttpResults.Created<Guid> createdResult)
        {
            if (context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<InvalidateCacheFilterMetadata>() is { } meta)
            {
                var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
                if (!string.IsNullOrEmpty(meta.Pattern))
                    await cacheService.RemoveCachePattern(meta.Pattern);
            }
        }

        if (result is Microsoft.AspNetCore.Http.HttpResults.NoContent notContentRest)
        {
            if (context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<InvalidateCacheFilterMetadata>() is { } meta)
            {
                var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
                if (!string.IsNullOrEmpty(meta.Pattern))
                    await cacheService.RemoveCachePattern(meta.Pattern);
            }
        }

        return result;
    }
}
