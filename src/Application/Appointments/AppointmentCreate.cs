namespace SkorinosGimnazija.Application.Appointments;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities.Appointments;
using Domain.Entities.Identity;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Validators;
using ValidationException = Common.Exceptions.ValidationException;

public static class AppointmentCreate
{
    public record Command(AppointmentCreateDto Appointment) : IRequest<AppointmentDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator(ICurrentUserService currentUserService)
        {
            RuleFor(v => v.Appointment).NotNull().SetValidator(new AppointmentCreateValidator(currentUserService));
        }
    }
     
    public class Handler : IRequestHandler<Command, AppointmentDto>
    {
        private readonly ICalendarService _calendarService;
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public Handler(
            IAppDbContext context,
            IMapper mapper,
            ICalendarService calendarService,
            IEmployeeService employeeService,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _calendarService = calendarService;
            _employeeService = employeeService;
            _currentUserService = currentUserService;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<AppointmentDto> Handle(Command request, CancellationToken _)
        {
            var date = await GetDateAsync(request.Appointment.DateId, request.Appointment.UserName);
            var host = await GetHostAsync(date.Type.Id, request.Appointment.UserName);
            var attendee = await GetAttendeeAsync(_currentUserService.UserName);

            var transaction = await _context.BeginTransactionAsync();

            var entity = _context.Appointments.Add(_mapper.Map<Appointment>(request.Appointment)).Entity;

            entity.UserDisplayName = host.FullName;
            entity.AttendeeEmail = attendee.Email;
            entity.AttendeeName = attendee.FullName;
            entity.AttendeeUserName = attendee.Id;

            await _context.SaveChangesAsync();

            var attendees = new List<string> { attendee.Email, host.Email };
            if (date.Type.InvitePrincipal)
            {
                attendees.Add((await _employeeService.GetPrincipalAsync()).Email);
            }

            var appointment = await _calendarService.AddAppointmentAsync(new()
            {
                Title = date.Type.Name,
                Description = $"{host.FullName} // {attendee.FullName}",
                IsOnline = date.Type.IsOnline,
                StartDate = date.Date,
                EndDate = date.Date.AddMinutes(date.Type.DurationInMinutes),
                AttendeeEmails = attendees
            });

            _mapper.Map(appointment, entity);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return _mapper.Map<AppointmentDto>(entity);
        }

        private async Task<Employee> GetHostAsync(int typeId, string hostUserName)
        {
            var host = await _employeeService.GetEmployeeAsync(hostUserName);
            if (host is null)
            {
                throw new ValidationException(nameof(hostUserName), "Invalid host");
            }

            var exclusiveHosts = await _context.AppointmentExclusiveHosts.AsNoTracking()
                                     .Where(x => x.TypeId == typeId)
                                     .Select(x => x.UserName)
                                     .ToListAsync();

            if (exclusiveHosts.Any() && !exclusiveHosts.Contains(hostUserName))
            {
                throw new ValidationException(nameof(hostUserName), "Invalid host");
            }

            return host;
        }

        private async Task<Employee> GetAttendeeAsync(string userName)
        {
            var attendee = await _employeeService.GetEmployeeAsync(userName);
            if (attendee is null)
            {
                throw new ValidationException(nameof(userName), "Invalid attendee");
            }

            return attendee;
        }

        private async Task<AppointmentDate> GetDateAsync(int dateId, string hostUserName)
        {
            var reservedDatesQuery = _context.AppointmentReservedDates.AsNoTracking()
                .Where(x => x.UserName == hostUserName)
                .Select(x => x.DateId);

            var date = await _context.AppointmentDates.AsNoTracking()
                           .Include(x => x.Type)
                           .FirstOrDefaultAsync(x =>
                               x.Id == dateId &&
                               !x.Type.IsPublic &&
                               x.Date > DateTime.UtcNow.AddHours(9) &&
                               x.Type.RegistrationEnd > DateTime.UtcNow &&
                               !reservedDatesQuery.Contains(x.Id));

            if (date is null)
            {
                throw new ValidationException(nameof(dateId), "Invalid date");
            }

            return date;
        }
    }
}