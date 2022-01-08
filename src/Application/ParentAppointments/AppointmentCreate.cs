namespace SkorinosGimnazija.Application.ParentAppointments;
using AutoMapper;
using MediatR;
using SkorinosGimnazija.Application.Appointments.Dtos;
using SkorinosGimnazija.Application.Common.Interfaces;
using SkorinosGimnazija.Application.Courses.Validators;

using SkorinosGimnazija.Domain.Entities.Bullies;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Appointments;
using Domain.Entities.Identity;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Validators;
using ValidationException = Common.Exceptions.ValidationException;
using FluentValidation.Results;
 
public  static class AppointmentCreate
{
    public record Command(AppointmentCreateDto Appointment) : IRequest<AppointmentDto>;
     
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Appointment).NotNull().SetValidator(new AppointmentCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, AppointmentDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICalendarService _calendarService;
        private readonly IEmployeeService _employeeService;
        private readonly ICurrentUserService _currentUserService;

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
            var date = await GetDate(request.Appointment.DateId);
            var teacher = await GetTeacher(request.Appointment.UserName);
            var attendee = await GetTeacher(_currentUserService.UserName);
              
            var transaction = await _context.BeginTransactionAsync();

            var entity = _context.Appointments.Add(_mapper.Map<Appointment>(request.Appointment)).Entity;

            entity.AttendeeEmail = attendee.Email;
            entity.AttendeeName = attendee.FullName;
            
            await _context.SaveChangesAsync();

            var attendees = new List<string> { attendee.Email, teacher.Email };
            if (date.Type.InvitePrincipal)
            {
                attendees.Add((await _employeeService.GetPrincipalAsync()).Email);
            }
             
            entity.EventId = await _calendarService.AddAppointmentAsync(
                                 date.Type.Name,
                                 $"{teacher.FullName} // {attendee.FullName}",
                                 date.Date,
                                 date.Date.AddMinutes(date.Type.DurationInMinutes),
                                 attendees.ToArray());

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return _mapper.Map<AppointmentDto>(entity);
        }

        private async Task<Employee> GetTeacher(string userName)
        {
            var teacher = await _employeeService.GetEmployeeAsync(userName);
            if (teacher is null)
            {
                throw new ValidationException(nameof(userName), "Invalid user name");
            }
             
            return teacher;
        }

        private async Task<AppointmentDate> GetDate(int dateId)
        {
         var date=   await _context.AppointmentDates.AsNoTracking()
                .Include(x => x.Type)
                .FirstOrDefaultAsync(x =>
                    x.Id == dateId);

            if (date is null)
            {
                throw new ValidationException(nameof(dateId), "Invalid date");
            }

            return date;
        }

    }
}