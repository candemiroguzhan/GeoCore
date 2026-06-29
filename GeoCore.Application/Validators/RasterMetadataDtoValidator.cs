using FluentValidation;
using GeoCore.Application.DTOs;

namespace GeoCore.Application.Validators;

public sealed class RasterMetadataDtoValidator : AbstractValidator<RasterMetadataDto>
{
    public RasterMetadataDtoValidator()
    {
        RuleFor(dto => dto.FileHash).NotEmpty();
        RuleFor(dto => dto.Width).GreaterThan(0);
        RuleFor(dto => dto.Height).GreaterThan(0);
        RuleFor(dto => dto.BandCount).GreaterThan(0);
        RuleFor(dto => dto.Srid).InclusiveBetween(1, 999999);
        RuleFor(dto => dto.Extent).NotNull().SetValidator(new BoundingBoxDtoValidator());
    }
}
