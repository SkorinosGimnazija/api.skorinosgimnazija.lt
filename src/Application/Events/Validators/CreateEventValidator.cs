namespace SkorinosGimnazija.Application.Events.Validators;

using Dtos;
using FluentValidation;

internal class CreateEventValidator : AbstractValidator<EventCreateDto>
{
    public CreateEventValidator()
    {
    }
}