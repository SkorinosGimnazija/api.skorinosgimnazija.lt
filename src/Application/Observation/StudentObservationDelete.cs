namespace SkorinosGimnazija.Application.Observation;

using Common.Exceptions;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class StudentObservationDelete
{
    public record Command(int Id) : IRequest<Unit>;

    public class Handler(IAppDbContext context, ICurrentUserService currentUser) : IRequestHandler<Command, Unit>
    {
        public async Task<Unit> Handle(Command request, CancellationToken ct)
        {
            var entity = await context.StudentObservations.FindAsync([request.Id], ct);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            if (!currentUser.IsOwnerOrManager(entity.TeacherId))
            {
                throw new UnauthorizedAccessException();
            }

            context.StudentObservations.Remove(entity);
            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}