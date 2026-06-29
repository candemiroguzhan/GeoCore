using FluentAssertions;
using GeoCore.Application.DTOs;
using GeoCore.Application.Mapping;
using GeoCore.Domain;
using GeoCore.Domain.Entities;
using GeoCore.Domain.Enums;
using Mapster;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GeoCore.Tests;

public sealed class MappingTests
{
    [Fact]
    public void Sample_place_maps_to_dto_with_geometry_wkt()
    {
        var config = new TypeAdapterConfig();
        new GeoCoreMappingRegister().Register(config);
        var place = new SamplePlace
        {
            Id = Guid.NewGuid(),
            Name = "A",
            Type = PlaceType.PointOfInterest,
            Srid = SpatialReferenceSystems.Wgs84,
            Geometry = CreatePoint(30, 10)
        };

        var dto = place.Adapt<SamplePlaceDto>(config);

        dto.Name.Should().Be("A");
        dto.Geometry.Srid.Should().Be(SpatialReferenceSystems.Wgs84);
        dto.Geometry.Wkt.Should().Be("POINT (30 10)");
    }

    [Fact]
    public void Geometry_dto_maps_to_geometry()
    {
        var config = new TypeAdapterConfig();
        new GeoCoreMappingRegister().Register(config);

        var geometry = new GeometryDto("POINT (30 10)", SpatialReferenceSystems.Wgs84).Adapt<Geometry>(config);

        geometry.SRID.Should().Be(SpatialReferenceSystems.Wgs84);
        geometry.AsText().Should().Be("POINT (30 10)");
    }

    private static Point CreatePoint(double x, double y)
    {
        var factory = NtsGeometryServices.Instance.CreateGeometryFactory(SpatialReferenceSystems.Wgs84);
        return factory.CreatePoint(new Coordinate(x, y));
    }
}
