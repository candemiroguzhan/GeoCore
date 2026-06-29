using GeoCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeoCore.Infrastructure.Persistence;

public class GeoCoreDbContext : DbContext
{
    public GeoCoreDbContext(DbContextOptions<GeoCoreDbContext> options) : base(options)
    {
    }

    public DbSet<SamplePlace> SamplePlaces => Set<SamplePlace>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GeoCoreDbContext).Assembly);
    }
}
