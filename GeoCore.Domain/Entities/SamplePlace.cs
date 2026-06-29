using GeoCore.Domain.Abstractions;
using GeoCore.Domain.Enums;
using NetTopologySuite.Geometries;

namespace GeoCore.Domain.Entities;

/// <summary>
/// Sample geometry-backed entity that consumers can mirror in their own domain.
/// </summary>
public sealed class SamplePlace : AuditableEntity, IHasGeometry
{
    public string Name { get; set; } = string.Empty;

    public PlaceType Type { get; set; } = PlaceType.Unknown;

    public int Srid { get; set; } = SpatialReferenceSystems.Wgs84;

    public Geometry Geometry { get; set; } = GeometryFactory.Default.CreatePoint();
}
