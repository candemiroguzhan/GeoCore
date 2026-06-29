using GeoCore.Application.DTOs;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace GeoCore.Application.Mapping;

public static class GeometryMapping
{
    public static Geometry ToGeometry(GeometryDto dto)
    {
        var geometry = new WKTReader().Read(dto.Wkt);
        geometry.SRID = dto.Srid;
        return geometry;
    }

    public static GeometryDto ToDto(Geometry geometry)
    {
        return new GeometryDto(new WKTWriter().Write(geometry), geometry.SRID);
    }
}
