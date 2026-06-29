using GeoCore.Domain;
using GeoCore.Domain.Entities;
using GeoCore.Domain.Enums;
using GeoCore.Domain.ValueObjects;
using GeoCore.Infrastructure.Persistence;
using GeoCore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GeoCore.Tests;

public sealed class RepositoryTests
{
    [Fact]
    public async Task Generic_repository_supports_crud_and_pagination()
    {
        await using var dbContext = CreateDbContext();
        var repository = new EfRepository<SamplePlace>(dbContext);

        await repository.AddRangeAsync([
            CreatePlace("A", 0, 0),
            CreatePlace("B", 1, 1),
            CreatePlace("C", 2, 2)
        ]);
        await dbContext.SaveChangesAsync();

        var count = await repository.CountAsync();
        var page = await repository.GetPagedAsync(new(1, 2));
        var exists = await repository.ExistsAsync(place => place.Name == "B");

        Assert.Equal(3, count);
        Assert.Equal(2, page.Items.Count);
        Assert.True(page.HasNextPage);
        Assert.True(exists);
    }

    [Fact]
    public async Task Geometry_repository_filters_by_bounding_box_and_distance()
    {
        await using var dbContext = CreateDbContext();
        var repository = new EfGeometryRepository<SamplePlace>(dbContext);

        await repository.AddRangeAsync([
            CreatePlace("Inside", 1, 1),
            CreatePlace("Outside", 20, 20)
        ]);
        await dbContext.SaveChangesAsync();

        var inBox = await repository.InBoundingBoxAsync(new BoundingBox(0, 0, 5, 5));
        var close = await repository.WithinDistanceAsync(CreatePoint(0, 0), 2);

        Assert.Single(inBox);
        Assert.Equal("Inside", inBox[0].Name);
        Assert.Single(close);
    }

    private static GeoCoreDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<GeoCoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new GeoCoreDbContext(options);
    }

    private static SamplePlace CreatePlace(string name, double x, double y)
    {
        return new SamplePlace
        {
            Name = name,
            Type = PlaceType.PointOfInterest,
            Srid = SpatialReferenceSystems.Wgs84,
            Geometry = CreatePoint(x, y)
        };
    }

    private static Point CreatePoint(double x, double y)
    {
        var factory = NtsGeometryServices.Instance.CreateGeometryFactory(SpatialReferenceSystems.Wgs84);
        return factory.CreatePoint(new Coordinate(x, y));
    }
}
