using GeoCore.Application.DTOs;
using GeoCore.Domain.Entities;
using GeoCore.Domain.ValueObjects;
using Mapster;
using NetTopologySuite.Geometries;

namespace GeoCore.Application.Mapping;

public sealed class GeoCoreMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GeometryDto, Geometry>()
            .MapWith(dto => GeometryMapping.ToGeometry(dto));

        config.NewConfig<Geometry, GeometryDto>()
            .MapWith(geometry => GeometryMapping.ToDto(geometry));

        config.NewConfig<BoundingBox, BoundingBoxDto>();
        config.NewConfig<BoundingBoxDto, BoundingBox>();

        config.NewConfig<SamplePlace, SamplePlaceDto>()
            .Map(destination => destination.Geometry, source => GeometryMapping.ToDto(source.Geometry));

        config.NewConfig<SamplePlaceDto, SamplePlace>()
            .Map(destination => destination.Geometry, source => GeometryMapping.ToGeometry(source.Geometry))
            .Map(destination => destination.Srid, source => source.Geometry.Srid);

        config.NewConfig<RasterMetadataDto, RasterMetadataDto>();
        config.NewConfig<TileMetadataDto, TileMetadataDto>();
        config.NewConfig<SpatialQueryRequestDto, SpatialQueryRequestDto>();
    }
}
