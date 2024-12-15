namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Validators;

public static class StudentObservationEdit
{
    public record Command(StudentObservationEditDto StudentObservation) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.StudentObservation).NotNull().SetValidator(new StudentObservationEditValidator());
        }
    }

    public class Handler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUser)
        : IRequestHandler<Command, Unit>
    {
        public async Task<Unit> Handle(Command request, CancellationToken ct)
        {
            var entity = await context.StudentObservations
                             .Include(x => x.Types)
                             .FirstOrDefaultAsync(x => x.Id == request.StudentObservation.Id, ct);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            if (!currentUser.IsOwnerOrManager(entity.TeacherId))
            {
                throw new UnauthorizedAccessException();
            }

            mapper.Map(request.StudentObservation, entity);

            entity.Types = await context.ObservationTypes
                               .Where(x => request.StudentObservation.TypeIds.Contains(x.Id))
                               .ToListAsync(ct);

            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}