using FluentValidation;
using GeoCore.Application.DTOs;
using GeoCore.Application.Mapping;
using NetTopologySuite.Geometries;

namespace GeoCore.Application.Validators;

public class GeometryDtoValidator : AbstractValidator<GeometryDto>
{
    public GeometryDtoValidator()
    {
        RuleFor(dto => dto.Wkt)
            .NotEmpty()
            .WithMessage("Geometry WKT is required.");

        RuleFor(dto => dto.Srid)
            .InclusiveBetween(1, 999999)
            .WithMessage("SRID must be a positive EPSG-compatible value.");

        RuleFor(dto => dto)
            .Must(TryParseGeometry)
            .WithMessage("Geometry WKT is invalid.")
            .When(dto => !string.IsNullOrWhiteSpace(dto.Wkt));

        RuleFor(dto => dto)
            .Must(BeValidGeometry)
            .WithMessage("Geometry topology is invalid.")
            .When(dto => !string.IsNullOrWhiteSpace(dto.Wkt));

        RuleFor(dto => dto)
            .Must(HaveCoordinatesInRange)
            .WithMessage("Geometry coordinates must be inside longitude and latitude ranges.")
            .When(dto => !string.IsNullOrWhiteSpace(dto.Wkt));
    }

    protected static Geometry? ParseOrNull(GeometryDto dto)
    {
        try
        {
            return GeometryMapping.ToGeometry(dto);
        }
        catch
        {
            return null;
        }
    }

    private static bool TryParseGeometry(GeometryDto dto)
    {
        return ParseOrNull(dto) is not null;
    }

    private static bool BeValidGeometry(GeometryDto dto)
    {
        return ParseOrNull(dto)?.IsValid == true;
    }

    private static bool HaveCoordinatesInRange(GeometryDto dto)
    {
        var geometry = ParseOrNull(dto);
        return geometry is not null && geometry.Coordinates.All(coordinate =>
            coordinate.X is >= -180 and <= 180 &&
            coordinate.Y is >= -90 and <= 90);
    }
}
