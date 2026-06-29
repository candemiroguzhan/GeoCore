using FluentAssertions;
using GeoCore.Application.DTOs;
using GeoCore.Application.Validators;
using GeoCore.Domain;

namespace GeoCore.Tests;

public sealed class ValidationTests
{
    [Fact]
    public void Geometry_validator_rejects_invalid_coordinate_ranges()
    {
        var validator = new GeometryDtoValidator();

        var result = validator.Validate(new GeometryDto("POINT (300 95)", SpatialReferenceSystems.Wgs84));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Polygon_validator_accepts_valid_polygon()
    {
        var validator = new PolygonGeometryDtoValidator();

        var result = validator.Validate(new GeometryDto("POLYGON ((0 0, 1 0, 1 1, 0 0))", SpatialReferenceSystems.Wgs84));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Bounding_box_validator_rejects_inverted_box()
    {
        var validator = new BoundingBoxDtoValidator();

        var result = validator.Validate(new BoundingBoxDto(10, 10, 5, 5, SpatialReferenceSystems.Wgs84));

        result.IsValid.Should().BeFalse();
    }
}
