using NetTopologySuite.Geometries;

namespace GeoCore.Domain.Abstractions;

/// <summary>
/// Marks an entity as addressable by spatial repository operations.
/// </summary>
public interface IHasGeometry
{
    Geometry Geometry { get; set; }
}
