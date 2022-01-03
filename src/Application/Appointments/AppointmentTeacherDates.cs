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
    public record Query(string UserName) : IRequest<List<AppointmentDateDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(50);
        }
    }
        
    public class Handler : IRequestHandler<Query, List<AppointmentDateDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public Handler(
            IAppDbContext context, IMapper mapper, IIdentityService identityService)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }
         
        public async Task<List<AppointmentDateDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var teacher =await  _identityService.GetOrCreateUserAsync(request.UserName);
            if (teacher is null)
            {
                return new();
            }
             
            var registeredDates = _context.ParentAppointments.AsNoTracking()
                                    .Where(x => x.TeacherId == teacher.Id)
                                    .Select(x => x.DateId);

            return await _context.ParentAppointmentDates.AsNoTracking()
                       .Where(x =>
                           x.Date > DateTime.Now &&
                           !registeredDates.Contains(x.Id))
                       .ProjectTo<AppointmentDateDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }


}
