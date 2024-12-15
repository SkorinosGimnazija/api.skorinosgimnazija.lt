namespace SkorinosGimnazija.Application.Observation;

using Common.Exceptions;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class ObservationTypeDelete
{
    public record Command(int Id) : IRequest<Unit>;

    public class Handler(IAppDbContext context) : IRequestHandler<Command, Unit>
    {
        public async Task<Unit> Handle(Command request, CancellationToken ct)
        {
            var entity = await context.ObservationTypes.FindAsync([request.Id], ct);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            context.ObservationTypes.Remove(entity);
            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}