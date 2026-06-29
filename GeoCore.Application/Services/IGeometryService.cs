using GeoCore.Domain.ValueObjects;
using NetTopologySuite.Geometries;

namespace GeoCore.Application.Services;

public interface IGeometryService
{
    Geometry FromWkt(string wkt, int srid);

    string ToWkt(Geometry geometry);

    Geometry FromWkb(byte[] wkb, int srid);

    byte[] ToWkb(Geometry geometry);

    Geometry FromGeoJson(string geoJson, int srid);

    string ToGeoJson(Geometry geometry);

    bool IsValid(Geometry geometry);

    Geometry SetSrid(Geometry geometry, int srid);

    Geometry Buffer(Geometry geometry, double distance);

    Geometry Union(Geometry left, Geometry right);

    Geometry Difference(Geometry left, Geometry right);

    Geometry Intersection(Geometry left, Geometry right);

    BoundingBox GetEnvelope(Geometry geometry);

    double Distance(Geometry left, Geometry right);

    double Area(Geometry geometry);

    double Length(Geometry geometry);

    Geometry Normalize(Geometry geometry);

    Geometry Simplify(Geometry geometry, double distanceTolerance);
}
