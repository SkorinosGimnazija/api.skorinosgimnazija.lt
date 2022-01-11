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
using AutoMapper.QueryableExtensions;
using Domain.Entities.Appointments;
using Domain.Entities.Identity;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Validators;
using ValidationException = Common.Exceptions.ValidationException;
using FluentValidation.Results;

public  static class AppointmentPublicCreate
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
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICalendarService _calendarService;
        private readonly IEmployeeService _employeeService;

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
            var date = await GetDateAsync(request.Appointment.DateId);
            var teacher = await GetTeacherAsync(request.Appointment.UserName);

            var transaction = await _context.BeginTransactionAsync();

            var entity = _context.Appointments.Add(_mapper.Map<Appointment>(request.Appointment)).Entity;
            await _context.SaveChangesAsync();

            entity.EventId = await _calendarService.AddAppointmentAsync(
                                 date.Type.Name,
                                 $"{teacher.FullName} // {request.Appointment.AttendeeName}",
                                 date.Date,
                                 date.Date.AddMinutes(date.Type.DurationInMinutes),
                                 request.Appointment.AttendeeEmail,
                                 teacher.Email);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return _mapper.Map<AppointmentDto>(entity);
        }

        private async Task<Employee> GetTeacherAsync(string userName)
        {
            var teacher = await _employeeService.GetEmployeeAsync(userName);
            if (teacher is null)
            {
                throw new ValidationException(nameof(userName), "Invalid user name");
            }
             
            return teacher;
        }

        private async Task<AppointmentDate> GetDateAsync(int dateId)
        {
            var date = await _context.AppointmentDates.AsNoTracking()
                           .Include(x => x.Type)
                           .FirstOrDefaultAsync(x =>
                               x.Id == dateId &&
                               x.Type.IsPublic);

            if (date is null || DateTime.Now >= date.Type.RegistrationEnd)
            {
                throw new ValidationException(nameof(dateId), "Invalid date");
            }
            
            return date;
        }

    }
}