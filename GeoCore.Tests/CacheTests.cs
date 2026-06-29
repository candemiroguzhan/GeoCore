using FluentAssertions;
using GeoCore.Application.Caching;
using GeoCore.Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace GeoCore.Tests;

public sealed class CacheTests
{
    [Fact]
    public async Task Cache_service_supports_get_set_exists_remove_and_get_or_set()
    {
        IDistributedCache distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        ICacheService cache = new RedisCacheService(distributedCache);
        var key = "geocore:test:item";

        await cache.SetAsync(key, new CacheSample("a"), new CacheEntryOptions(TimeSpan.FromMinutes(5)));

        (await cache.ExistsAsync(key)).Should().BeTrue();
        (await cache.GetAsync<CacheSample>(key)).Should().Be(new CacheSample("a"));

        var value = await cache.GetOrSetAsync("geocore:test:getorset", _ => Task.FromResult(new CacheSample("b")));
        value.Should().Be(new CacheSample("b"));

        await cache.RemoveAsync(key);
        (await cache.ExistsAsync(key)).Should().BeFalse();
    }

    [Fact]
    public void Cache_key_builder_creates_deterministic_namespaced_keys()
    {
        var builder = new GeoCacheKeyBuilder();
        var geometryHash = builder.Hash("POINT (30 10)");

        var key = builder.ForCoordinateTransform(4326, 3857, geometryHash);

        key.Should().Be($"geocore:spatial:transform:4326:3857:{geometryHash}");
        builder.ForOtbResult("BandMath", "ABC").Should().Be("geocore:otb:result:bandmath:abc");
    }

    private sealed record CacheSample(string Value);
}
