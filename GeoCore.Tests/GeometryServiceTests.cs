using GeoCore.Domain;
using GeoCore.Infrastructure.Services;

namespace GeoCore.Tests;

public sealed class GeometryServiceTests
{
    [Fact]
    public void Wkt_round_trip_preserves_geometry_and_srid()
    {
        var service = new GeometryService();

        var geometry = service.FromWkt("POINT (30 10)", SpatialReferenceSystems.Wgs84);
        var wkt = service.ToWkt(geometry);

        Assert.Equal(SpatialReferenceSystems.Wgs84, geometry.SRID);
        Assert.Equal("POINT (30 10)", wkt);
    }

    [Fact]
    public void Buffer_creates_area_around_point()
    {
        var service = new GeometryService();
        var point = service.FromWkt("POINT (0 0)", SpatialReferenceSystems.Wgs84);

        var buffered = service.Buffer(point, 10);

        Assert.True(buffered.Area > 0);
        Assert.True(service.IsValid(buffered));
    }
}
