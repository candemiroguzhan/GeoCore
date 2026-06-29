using System.Text.Json;
using GeoCore.Application.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace GeoCore.Infrastructure.Caching;

public sealed class RedisCacheService : IDistributedCacheService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var json = await _cache.GetStringAsync(key, cancellationToken);
        return string.IsNullOrWhiteSpace(json) ? default : JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    public async Task SetAsync<T>(string key, T value, CacheEntryOptions? options = null, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(value, JsonOptions);
        await _cache.SetStringAsync(key, json, ToDistributedOptions(options), cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        return _cache.RemoveAsync(key, cancellationToken);
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _cache.GetAsync(key, cancellationToken) is not null;
    }

    public async Task<T> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var cached = await GetAsync<T>(key, cancellationToken);
        if (cached is not null)
        {
            return cached;
        }

        var value = await factory(cancellationToken);
        await SetAsync(key, value, options, cancellationToken);
        return value;
    }

    private static DistributedCacheEntryOptions ToDistributedOptions(CacheEntryOptions? options)
    {
        return new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = options?.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = options?.SlidingExpiration
        };
    }
}
