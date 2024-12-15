namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Validators;

public static class ObservationTypeEdit
{
    public record Command(ObservationTypeEditDto ObservationType) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.ObservationType).NotNull().SetValidator(new ObservationTypeEditValidator());
        }
    }

    public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Unit>
    {
        public async Task<Unit> Handle(Command request, CancellationToken ct)
        {
            var entity = await context.ObservationTypes.FindAsync([request.ObservationType.Id], ct);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            mapper.Map(request.ObservationType, entity);

            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}