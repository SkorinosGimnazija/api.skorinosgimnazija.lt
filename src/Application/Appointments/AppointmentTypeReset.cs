namespace SkorinosGimnazija.Application.Appointments;

using System.Diagnostics.CodeAnalysis;
using Common.Exceptions;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AppointmentTypeReset
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
            var type = await _context.AppointmentTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (type is null)
            {
                throw new NotFoundException();
            }

            await _context.Appointments.Where(x => x.Date.TypeId == type.Id).ExecuteDeleteAsync();
            await _context.AppointmentExclusiveHosts.Where(x => x.TypeId == type.Id).ExecuteDeleteAsync();
            await _context.AppointmentDates.Where(x => x.TypeId == type.Id).ExecuteDeleteAsync();

            return Unit.Value;
        }
    }
}