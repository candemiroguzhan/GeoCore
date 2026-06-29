using FluentValidation;
using GeoCore.Application.DTOs;
using NetTopologySuite.Geometries;

namespace GeoCore.Application.Validators;

public sealed class PolygonGeometryDtoValidator : GeometryDtoValidator
{
    public PolygonGeometryDtoValidator()
    {
        RuleFor(dto => dto)
            .Must(dto => ParseOrNull(dto) is Polygon or MultiPolygon)
            .WithMessage("Geometry must be a polygon or multipolygon.");

        RuleFor(dto => dto)
            .Must(HaveClosedPolygonRings)
            .WithMessage("Polygon rings must be closed.");
    }

    private static bool HaveClosedPolygonRings(GeometryDto dto)
    {
        return ParseOrNull(dto) switch
        {
            Polygon polygon => IsClosed(polygon),
            MultiPolygon multiPolygon => multiPolygon.Geometries.OfType<Polygon>().All(IsClosed),
            _ => false
        };
    }

    private static bool IsClosed(Polygon polygon)
    {
        return polygon.ExteriorRing.IsClosed &&
               Enumerable.Range(0, polygon.NumInteriorRings).All(index => polygon.GetInteriorRingN(index).IsClosed);
    }
}
