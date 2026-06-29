using GeoCore.Domain.Enums;

namespace GeoCore.Application.DTOs;

public sealed record SamplePlaceDto(Guid Id, string Name, PlaceType Type, GeometryDto Geometry);
