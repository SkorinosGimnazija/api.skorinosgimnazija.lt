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

public static class AppointmentPublicCreate
{
    public record Command(AppointmentPublicCreateDto Appointment) : IRequest<AppointmentDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator(ICaptchaService captchaService)
        {
            RuleFor(v => v.Appointment).NotNull().SetValidator(new AppointmentPublicCreateValidator(captchaService));
        }
    }

    public class Handler : IRequestHandler<Command, AppointmentDto>
    {
        private readonly ICalendarService _calendarService;
        private readonly IAppDbContext _context;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public Handler(
            IAppDbContext context,
            IMapper mapper,
            ICalendarService calendarService,
            IEmployeeService employeeService)
        {
            _context = context;
            _mapper = mapper;
            _calendarService = calendarService;
            _employeeService = employeeService;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<AppointmentDto> Handle(Command request, CancellationToken _)
        {
            var date = await GetDateAsync(request.Appointment.DateId, request.Appointment.UserName);
            var teacher = await GetHostAsync(date.Type.Id, request.Appointment.UserName);

            var transaction = await _context.BeginTransactionAsync();

            var entity = _context.Appointments.Add(_mapper.Map<Appointment>(request.Appointment)).Entity;

            entity.UserDisplayName = teacher.FullName;

            await _context.SaveChangesAsync();

            var appointment = await _calendarService.AddAppointmentAsync(
                                  date.Type.Name,
                                  $"{teacher.FullName} // {request.Appointment.AttendeeName}",
                                  date.Date,
                                  date.Date.AddMinutes(date.Type.DurationInMinutes),
                                  request.Appointment.AttendeeEmail,
                                  teacher.Email);

            entity.EventId = appointment.EventId;
            entity.EventMeetingLink = appointment.EventMeetingLink;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return _mapper.Map<AppointmentDto>(entity);
        }

        private async Task<Employee> GetHostAsync(int typeId, string hostUserName)
        {
            var employee = await _employeeService.GetEmployeeAsync(hostUserName);
            if (employee is null)
            {
                throw new ValidationException(nameof(hostUserName), "Invalid user name");
            }

            var exclusiveHosts = await _context.AppointmentExclusiveHosts.AsNoTracking()
                                     .Where(x => x.TypeId == typeId)
                                     .Select(x => x.UserName)
                                     .ToListAsync();

            if (exclusiveHosts.Any() && !exclusiveHosts.Contains(hostUserName))
            {
                throw new ValidationException(nameof(hostUserName), "Invalid host");
            }

            return employee;
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
                               x.Type.IsPublic &&
                               x.Date > DateTime.UtcNow.AddHours(3) &&
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