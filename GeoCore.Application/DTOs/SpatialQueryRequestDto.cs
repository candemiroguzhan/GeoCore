namespace GeoCore.Application.DTOs;

public sealed record SpatialQueryRequestDto(
    GeometryDto Geometry,
    BoundingBoxDto? BoundingBox,
    int PageNumber,
    int PageSize,
    double? Distance);
