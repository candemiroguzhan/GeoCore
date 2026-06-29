using GeoCore.Domain.ValueObjects;
using NetTopologySuite.Geometries;

namespace GeoCore.Infrastructure.Spatial;

public static class PostGisSpatialQueryHelpers
{
    public static BoundingBox ToBoundingBox(this Geometry geometry)
    {
        var envelope = geometry.EnvelopeInternal;
        return new BoundingBox(envelope.MinX, envelope.MinY, envelope.MaxX, envelope.MaxY, geometry.SRID);
    }

    public static Geometry EnsureSrid(this Geometry geometry, int srid)
    {
        if (geometry.SRID == srid)
        {
            return geometry;
        }

        var copy = (Geometry)geometry.Copy();
        copy.SRID = srid;
        return copy;
    }
}
