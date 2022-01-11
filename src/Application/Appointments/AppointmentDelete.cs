namespace SkorinosGimnazija.Application.ParentAppointments;
using MediatR;
using SkorinosGimnazija.Application.Common.Exceptions;

using SkorinosGimnazija.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public  static class AppointmentDelete
{
    public record Command(int Id) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly ICalendarService _calendarService;

        public Handler(IAppDbContext context, ICalendarService calendarService)
        {
            _context = context;
            _calendarService = calendarService;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            _context.Appointments.Remove(entity);
           
            await  _calendarService.DeleteAppointmentAsync(entity.EventId);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}