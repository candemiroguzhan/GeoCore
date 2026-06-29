namespace GeoCore.Application.DTOs;

public sealed record TileMetadataDto(string TileHash, int X, int Y, int Z, BoundingBoxDto Extent);
