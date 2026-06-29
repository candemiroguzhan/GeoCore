using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GeoCore.Domain.ValueObjects;

/// <summary>
/// Immutable axis-aligned bounding box.
/// </summary>
public readonly record struct BoundingBox(double MinX, double MinY, double MaxX, double MaxY, int Srid = SpatialReferenceSystems.Wgs84)
{
    public Envelope ToEnvelope() => new(MinX, MaxX, MinY, MaxY);

    public Polygon ToPolygon()
    {
        var factory = NtsGeometryServices.Instance.CreateGeometryFactory(Srid);
        return factory.CreatePolygon(
        [
            new Coordinate(MinX, MinY),
            new Coordinate(MaxX, MinY),
            new Coordinate(MaxX, MaxY),
            new Coordinate(MinX, MaxY),
            new Coordinate(MinX, MinY)
        ]);
    }
}
