namespace SkorinosGimnazija.Application.Courses.Validators;

using Dtos;
using FluentValidation;

internal class CourseCreateValidator : AbstractValidator<CourseCreateDto>
{
    public CourseCreateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(512);
        RuleFor(x => x.Organizer).NotEmpty().MaximumLength(256);
        RuleFor(x => x.CertificateNr).MaximumLength(100);
        RuleFor(x => x.DurationInHours).GreaterThan(0);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(x => x.StartDate);
    }
}