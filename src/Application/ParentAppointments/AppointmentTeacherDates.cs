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
using SkorinosGimnazija.Application.Courses.Validators;
using SkorinosGimnazija.Application.Appointments.Dtos;
 
public static class AppointmentTeacherDates
{
    public record Query(string TeachersUserName) : IRequest<List<AppointmentDateDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.TeachersUserName).NotEmpty().MaximumLength(100);
        }
    } 
        
    public class Handler : IRequestHandler<Query, List<AppointmentDateDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public Handler(
            IAppDbContext context, IMapper mapper, IEmployeeService employeeService)
        {
            _context = context;
            _mapper = mapper;
            _employeeService = employeeService;
        }
         
        public async Task<List<AppointmentDateDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var teacher = await _employeeService.GetEmployeeAsync(request.TeachersUserName);
            if (teacher is null)
            {
                throw new NotFoundException();
            }

            var reservedDatesQuery = _context.ParentAppointmentReservedDates
                .Where(x => x.UserName == request.TeachersUserName)
                .Select(x => x.DateId);

            var registeredDatesQuery = _context.ParentAppointments
                .Where(x => x.UserName == request.TeachersUserName)
                .Select(x => x.DateId);
             
            return await _context.ParentAppointmentDates.AsNoTracking()
                       .Where(x =>
                           x.Date > DateTime.Now &&
                           !registeredDatesQuery.Contains(x.Id) &&
                           !reservedDatesQuery.Contains(x.Id))
                       .ProjectTo<AppointmentDateDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }


}
