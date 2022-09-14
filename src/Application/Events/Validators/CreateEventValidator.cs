namespace SkorinosGimnazija.Application.Events.Validators;

using Dtos;
using FluentValidation;

internal class CreateEventValidator : AbstractValidator<EventCreateDto>
{
    public CreateEventValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(512);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(x => x.StartDate);
    }
}