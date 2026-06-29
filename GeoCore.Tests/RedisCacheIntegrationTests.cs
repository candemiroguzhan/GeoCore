using FluentAssertions;
using GeoCore.Application.Caching;
using GeoCore.Infrastructure.Caching;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Testcontainers.Redis;

namespace GeoCore.Tests;

public sealed class RedisCacheIntegrationTests
{
    [Fact]
    public async Task Redis_cache_service_round_trips_values_when_docker_is_available()
    {
        RedisContainer redis;

        try
        {
            redis = new RedisBuilder("redis:7-alpine").Build();
            await redis.StartAsync();
        }
        catch
        {
            return;
        }

        await using (redis)
        {
            var distributedCache = new RedisCache(Options.Create(new RedisCacheOptions
            {
                Configuration = redis.GetConnectionString(),
                InstanceName = "geocore:"
            }));
            IDistributedCacheService cache = new RedisCacheService(distributedCache);

            await cache.SetAsync("geocore:testcontainers:redis", new RedisSample("ok"));

            var value = await cache.GetAsync<RedisSample>("geocore:testcontainers:redis");

            value.Should().Be(new RedisSample("ok"));
        }
    }

    private sealed record RedisSample(string Value);
}
