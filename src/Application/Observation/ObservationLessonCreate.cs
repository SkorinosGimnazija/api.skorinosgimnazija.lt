namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using Common.Interfaces;
using Domain.Entities.Observation;
using Dtos;
using FluentValidation;
using MediatR;
using Validators;

public static class ObservationLessonCreate
{
    public record Command(ObservationLessonCreateDto Lesson) : IRequest<ObservationLessonDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Lesson).NotNull().SetValidator(new ObservationLessonCreateValidator());
        }
    }

    public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, ObservationLessonDto>
    {
        public async Task<ObservationLessonDto> Handle(Command request, CancellationToken ct)
        {
            var entity = context.ObservationLessons.Add(mapper.Map<ObservationLesson>(request.Lesson)).Entity;

            await context.SaveChangesAsync(ct);

            return mapper.Map<ObservationLessonDto>(entity);
        }
    }
}