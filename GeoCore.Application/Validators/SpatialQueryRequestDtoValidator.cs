using FluentValidation;
using GeoCore.Application.DTOs;

namespace GeoCore.Application.Validators;

public sealed class SpatialQueryRequestDtoValidator : AbstractValidator<SpatialQueryRequestDto>
{
    public SpatialQueryRequestDtoValidator()
    {
        RuleFor(request => request.Geometry)
            .NotNull()
            .SetValidator(new GeometryDtoValidator());

        RuleFor(request => request.BoundingBox!)
            .SetValidator(new BoundingBoxDtoValidator())
            .When(request => request.BoundingBox is not null);

        RuleFor(request => request.PageNumber).GreaterThan(0);
        RuleFor(request => request.PageSize).InclusiveBetween(1, 500);
        RuleFor(request => request.Distance).GreaterThanOrEqualTo(0).When(request => request.Distance.HasValue);
    }
}
