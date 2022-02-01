namespace SkorinosGimnazija.Application.ParentAppointments;

using System.Diagnostics.CodeAnalysis;
using Common.Exceptions;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AppointmentTypeDelete
{
    public record Command(int Id) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;

        public Handler(IAppDbContext context)
        {
            _context = context;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.AppointmentTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            _context.AppointmentTypes.Remove(entity);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}