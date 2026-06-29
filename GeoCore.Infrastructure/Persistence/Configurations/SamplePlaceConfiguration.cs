using GeoCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeoCore.Infrastructure.Persistence.Configurations;

internal sealed class SamplePlaceConfiguration : IEntityTypeConfiguration<SamplePlace>
{
    public void Configure(EntityTypeBuilder<SamplePlace> builder)
    {
        builder.ToTable("sample_places");
        builder.HasKey(place => place.Id);

        builder.Property(place => place.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(place => place.Type)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(place => place.Srid)
            .IsRequired();

        builder.Property(place => place.Geometry)
            .HasColumnType("geometry")
            .IsRequired();

        builder.HasIndex(place => place.Geometry)
            .HasMethod("gist");
    }
}
