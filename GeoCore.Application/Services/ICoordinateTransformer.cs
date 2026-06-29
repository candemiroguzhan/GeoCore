using NetTopologySuite.Geometries;

namespace GeoCore.Application.Services;

public interface ICoordinateTransformer
{
    Task<Geometry> TransformAsync(Geometry geometry, int targetSrid, CancellationToken cancellationToken = default);
}
