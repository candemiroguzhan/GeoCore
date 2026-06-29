using FluentValidation;
using GeoCore.Application.DTOs;

namespace GeoCore.Application.Validators;

public sealed class BoundingBoxDtoValidator : AbstractValidator<BoundingBoxDto>
{
    public BoundingBoxDtoValidator()
    {
        RuleFor(box => box.Srid)
            .InclusiveBetween(1, 999999)
            .WithMessage("SRID must be a positive EPSG-compatible value.");

        RuleFor(box => box.MinX)
            .InclusiveBetween(-180, 180);

        RuleFor(box => box.MaxX)
            .InclusiveBetween(-180, 180);

        RuleFor(box => box.MinY)
            .InclusiveBetween(-90, 90);

        RuleFor(box => box.MaxY)
            .InclusiveBetween(-90, 90);

        RuleFor(box => box)
            .Must(box => box.MinX < box.MaxX && box.MinY < box.MaxY)
            .WithMessage("Bounding box minimum coordinates must be lower than maximum coordinates.");
    }
}
