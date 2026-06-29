namespace GeoCore.Application.DTOs;

public sealed record SpatialQueryResponseDto<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    GeometryDto? QueryGeometry,
    BoundingBoxDto? BoundingBox);
