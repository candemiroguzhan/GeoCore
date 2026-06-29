namespace GeoCore.Application.Caching;

public sealed record CacheEntryOptions(
    TimeSpan? AbsoluteExpirationRelativeToNow = null,
    TimeSpan? SlidingExpiration = null);
