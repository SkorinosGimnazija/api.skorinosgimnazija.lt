namespace SkorinosGimnazija.Application.Appointments;
using AutoMapper;
using MediatR;
using SkorinosGimnazija.Application.Common.Interfaces;
using SkorinosGimnazija.Application.Common.Pagination;

using SkorinosGimnazija.Application.Courses.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Extensions;
using Domain.Entities.Identity;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using ParentAppointments.Dtos;
using SkorinosGimnazija.Application.Courses.Validators;
using SkorinosGimnazija.Application.Appointments.Dtos;
using SkorinosGimnazija.Application.ParentAppointments.Validators;

public static class AppointmentDatesList
{
    public record Query(AppointmentDatesQuery Appointment) : IRequest<List<AppointmentDateDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator(IEmployeeService employeeService)
        {
            RuleFor(x => x.Appointment).NotNull().SetValidator(new AppointmentDatesQueryValidator(employeeService));
        }
    } 
          
    public class Handler : IRequestHandler<Query, List<AppointmentDateDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
         
        public async Task<List<AppointmentDateDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var appointmentType = await _context.AppointmentTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Slug == request.Appointment.AppointmentTypeSlug,
                cancellationToken);

            if (appointmentType is null)
            {
                throw new NotFoundException();
            }

            var reservedDatesQuery = _context.AppointmentReservedDates
                .Where(x => x.UserName == request.Appointment.UserName)
                .Select(x => x.DateId);
             
            var registeredDatesQuery = _context.Appointments
                .Where(x => x.UserName == request.Appointment.UserName)
                .Select(x => x.DateId);
             
            return await _context.AppointmentDates.AsNoTracking()
                       .Where(x =>
                           x.Date > DateTime.Now &&
                           x.TypeId == appointmentType.Id &&
                           !registeredDatesQuery.Contains(x.Id) &&
                           !reservedDatesQuery.Contains(x.Id))
                       .ProjectTo<AppointmentDateDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }


}
