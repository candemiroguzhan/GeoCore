namespace GeoCore.Application.DTOs;

public sealed record RasterMetadataDto(
    string FileHash,
    int Width,
    int Height,
    int BandCount,
    int Srid,
    BoundingBoxDto Extent);
