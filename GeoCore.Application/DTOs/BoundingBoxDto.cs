namespace GeoCore.Application.DTOs;

public sealed record BoundingBoxDto(double MinX, double MinY, double MaxX, double MaxY, int Srid);
