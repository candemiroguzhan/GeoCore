using GeoCore.Application.Repositories;
using GeoCore.Domain.Abstractions;
using GeoCore.Domain.Entities;
using GeoCore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace GeoCore.Infrastructure.Repositories;

public class EfGeometryRepository<T> : EfRepository<T>, IGeometryRepository<T>
    where T : BaseEntity, IHasGeometry
{
    public EfGeometryRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IReadOnlyList<T>> IntersectsAsync(Geometry geometry, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        return await Query(asTracking).Where(entity => entity.Geometry.Intersects(geometry)).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ContainsAsync(Geometry geometry, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        return await Query(asTracking).Where(entity => entity.Geometry.Contains(geometry)).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> WithinAsync(Geometry geometry, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        return await Query(asTracking).Where(entity => entity.Geometry.Within(geometry)).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> TouchesAsync(Geometry geometry, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        return await Query(asTracking).Where(entity => entity.Geometry.Touches(geometry)).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> OverlapsAsync(Geometry geometry, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        return await Query(asTracking).Where(entity => entity.Geometry.Overlaps(geometry)).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> WithinDistanceAsync(Geometry geometry, double distance, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        return await Query(asTracking).Where(entity => entity.Geometry.Distance(geometry) <= distance).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> InBoundingBoxAsync(BoundingBox boundingBox, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        var polygon = boundingBox.ToPolygon();
        return await Query(asTracking).Where(entity => entity.Geometry.Intersects(polygon)).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> WithAreaBetweenAsync(double minArea, double maxArea, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        return await Query(asTracking)
            .Where(entity => entity.Geometry.Area >= minArea && entity.Geometry.Area <= maxArea)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> WithSridAsync(int srid, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        return await Query(asTracking).Where(entity => entity.Geometry.SRID == srid).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> OrderByDistanceAsync(Geometry geometry, int take, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        return await Query(asTracking)
            .OrderBy(entity => entity.Geometry.Distance(geometry))
            .Take(Math.Max(0, take))
            .ToListAsync(cancellationToken);
    }
}
