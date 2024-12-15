namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Validators;

public static class ObservationTargetEdit
{
    public record Command(ObservationTargetEditDto ObservationTarget) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.ObservationTarget).NotNull().SetValidator(new ObservationTargetEditValidator());
        }
    }

    public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Unit>
    {
        public async Task<Unit> Handle(Command request, CancellationToken ct)
        {
            var entity = await context.ObservationTargets.FindAsync([request.ObservationTarget.Id], ct);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            mapper.Map(request.ObservationTarget, entity);

            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}