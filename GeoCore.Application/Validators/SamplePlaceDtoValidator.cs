using FluentValidation;
using GeoCore.Application.DTOs;

namespace GeoCore.Application.Validators;

public sealed class SamplePlaceDtoValidator : AbstractValidator<SamplePlaceDto>
{
    public SamplePlaceDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(dto => dto.Geometry).NotNull().SetValidator(new GeometryDtoValidator());
    }
}
