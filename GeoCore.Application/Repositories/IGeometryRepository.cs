using GeoCore.Domain.Abstractions;
using GeoCore.Domain.Entities;
using GeoCore.Domain.ValueObjects;
using NetTopologySuite.Geometries;

namespace GeoCore.Application.Repositories;

public interface IGeometryRepository<T> : IRepository<T>
    where T : BaseEntity, IHasGeometry
{
    Task<IReadOnlyList<T>> IntersectsAsync(Geometry geometry, bool asTracking = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> ContainsAsync(Geometry geometry, bool asTracking = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> WithinAsync(Geometry geometry, bool asTracking = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> TouchesAsync(Geometry geometry, bool asTracking = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> OverlapsAsync(Geometry geometry, bool asTracking = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> WithinDistanceAsync(Geometry geometry, double distance, bool asTracking = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> InBoundingBoxAsync(BoundingBox boundingBox, bool asTracking = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> WithAreaBetweenAsync(double minArea, double maxArea, bool asTracking = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> WithSridAsync(int srid, bool asTracking = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> OrderByDistanceAsync(Geometry geometry, int take, bool asTracking = false, CancellationToken cancellationToken = default);
}
