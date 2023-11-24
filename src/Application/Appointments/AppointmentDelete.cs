namespace SkorinosGimnazija.Application.Appointments;

using System.Diagnostics.CodeAnalysis;
using Common.Exceptions;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AppointmentDelete
{
    public record Command(int Id) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly ICalendarService _calendarService;
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public Handler(IAppDbContext context, ICalendarService calendarService, ICurrentUserService currentUserService)
        {
            _context = context;
            _calendarService = calendarService;
            _currentUser = currentUserService;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            if (!_currentUser.IsAdmin() && _currentUser.UserName != entity.AttendeeUserName)
            {
                throw new UnauthorizedAccessException();
            }

            var transaction = await _context.BeginTransactionAsync();

            _context.Appointments.Remove(entity);

            await _context.SaveChangesAsync();

            await _calendarService.DeleteAppointmentAsync(entity.EventId);

            await transaction.CommitAsync();

            return Unit.Value;
        }
    }
}