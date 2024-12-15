namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Validators;

public static class ObservationLessonEdit
{
    public record Command(ObservationLessonEditDto ObservationLesson) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.ObservationLesson).NotNull().SetValidator(new ObservationLessonEditValidator());
        }
    }

    public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Unit>
    {
        public async Task<Unit> Handle(Command request, CancellationToken ct)
        {
            var entity = await context.ObservationLessons.FindAsync([request.ObservationLesson.Id], ct);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            mapper.Map(request.ObservationLesson, entity);

            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}