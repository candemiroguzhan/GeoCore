using GeoCore.Application.Repositories;
using GeoCore.Application.Services;
using GeoCore.Application.Caching;
using GeoCore.Application.DependencyInjection;
using GeoCore.Infrastructure.Caching;
using GeoCore.Infrastructure.Persistence;
using GeoCore.Infrastructure.Repositories;
using GeoCore.Infrastructure.Resilience;
using GeoCore.Infrastructure.Scanning;
using GeoCore.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GeoCore.Infrastructure.DependencyInjection;

public static class GeoCoreInfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddGeoCoreInfrastructure(
        this IServiceCollection services,
        string connectionString,
        Action<DbContextOptionsBuilder>? configureOptions = null)
    {
        services.AddGeoCoreApplication();

        services.AddDbContext<GeoCoreDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql => npgsql.UseNetTopologySuite());
            configureOptions?.Invoke(options);
        });

        services.AddScoped<DbContext>(provider => provider.GetRequiredService<GeoCoreDbContext>());
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IGeometryRepository<>), typeof(EfGeometryRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IGeometryService, GeometryService>();
        services.AddSingleton<ICoordinateTransformer, NoOpCoordinateTransformer>();
        services.TryAddSingleton<IGeoCacheKeyBuilder, GeoCacheKeyBuilder>();
        services.TryAddSingleton<GeoCoreResiliencePipelineFactory>();

        services.Scan(scan => scan
            .FromAssemblies(typeof(GeometryService).Assembly)
            .AddClasses(classes => classes.AssignableTo<IGeoCoreProcessor>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo<IGeoCoreAdapter>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    public static IServiceCollection AddGeoCoreRedisCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis")
            ?? configuration["GeoCore:Redis:ConnectionString"];

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Redis connection string is missing. Configure ConnectionStrings:Redis or GeoCore:Redis:ConnectionString.");
        }

        return services.AddGeoCoreRedisCaching(connectionString);
    }

    public static IServiceCollection AddGeoCoreRedisCaching(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = "geocore:";
        });

        services.TryAddSingleton<IGeoCacheKeyBuilder, GeoCacheKeyBuilder>();
        services.AddScoped<IDistributedCacheService, RedisCacheService>();
        services.AddScoped<ICacheService>(provider => provider.GetRequiredService<IDistributedCacheService>());

        return services;
    }
}
