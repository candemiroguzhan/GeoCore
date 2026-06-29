using GeoCore.Application.Services;
using GeoCore.Domain.ValueObjects;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.Simplify;

namespace GeoCore.Infrastructure.Services;

public sealed class GeometryService : IGeometryService
{
    public Geometry FromWkt(string wkt, int srid)
    {
        var geometry = new WKTReader().Read(wkt);
        geometry.SRID = srid;
        return geometry;
    }

    public string ToWkt(Geometry geometry)
    {
        return new WKTWriter().Write(geometry);
    }

    public Geometry FromWkb(byte[] wkb, int srid)
    {
        var geometry = new WKBReader().Read(wkb);
        geometry.SRID = srid;
        return geometry;
    }

    public byte[] ToWkb(Geometry geometry)
    {
        return new WKBWriter().Write(geometry);
    }

    public Geometry FromGeoJson(string geoJson, int srid)
    {
        var geometry = new GeoJsonReader().Read<Geometry>(geoJson);
        geometry.SRID = srid;
        return geometry;
    }

    public string ToGeoJson(Geometry geometry)
    {
        return new GeoJsonWriter().Write(geometry);
    }

    public bool IsValid(Geometry geometry)
    {
        return geometry.IsValid;
    }

    public Geometry SetSrid(Geometry geometry, int srid)
    {
        var copy = (Geometry)geometry.Copy();
        copy.SRID = srid;
        return copy;
    }

    public Geometry Buffer(Geometry geometry, double distance)
    {
        return geometry.Buffer(distance);
    }

    public Geometry Union(Geometry left, Geometry right)
    {
        return left.Union(right);
    }

    public Geometry Difference(Geometry left, Geometry right)
    {
        return left.Difference(right);
    }

    public Geometry Intersection(Geometry left, Geometry right)
    {
        return left.Intersection(right);
    }

    public BoundingBox GetEnvelope(Geometry geometry)
    {
        var envelope = geometry.EnvelopeInternal;
        return new BoundingBox(envelope.MinX, envelope.MinY, envelope.MaxX, envelope.MaxY, geometry.SRID);
    }

    public double Distance(Geometry left, Geometry right)
    {
        return left.Distance(right);
    }

    public double Area(Geometry geometry)
    {
        return geometry.Area;
    }

    public double Length(Geometry geometry)
    {
        return geometry.Length;
    }

    public Geometry Normalize(Geometry geometry)
    {
        var copy = (Geometry)geometry.Copy();
        copy.Normalize();
        return copy;
    }

    public Geometry Simplify(Geometry geometry, double distanceTolerance)
    {
        return TopologyPreservingSimplifier.Simplify(geometry, distanceTolerance);
    }
}
