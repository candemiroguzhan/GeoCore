using GeoCore.Application.Services;
using NetTopologySuite.Geometries;

namespace GeoCore.Infrastructure.Services;

/// <summary>
/// Default transformer that only changes SRID metadata. Replace with a projection-aware implementation when needed.
/// </summary>
public sealed class NoOpCoordinateTransformer : ICoordinateTransformer
{
    public Task<Geometry> TransformAsync(Geometry geometry, int targetSrid, CancellationToken cancellationToken = default)
    {
        var copy = (Geometry)geometry.Copy();
        copy.SRID = targetSrid;
        return Task.FromResult(copy);
    }
}
