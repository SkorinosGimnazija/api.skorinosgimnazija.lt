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
using SkorinosGimnazija.Domain.Entities.Appointments;
using System.Threading;

public static class AppointmentAvailableDatesList
{
    public record Query(string TypeSlug, string UserName, bool IsPublic) : IRequest<List<AppointmentDateDto>>;
     
    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.TypeSlug).NotEmpty().MaximumLength(100);
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(100);
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
            var appointmentType = await GetTypeAsync(request.TypeSlug, request.IsPublic, cancellationToken);
            if (DateTime.Now >= appointmentType.RegistrationEnd)
            {
                return new();
            }

            var reservedDatesQuery = _context.AppointmentReservedDates
                .Where(x => x.UserName == request.UserName)
                .Select(x => x.DateId);
             
            var registeredDatesQuery = _context.Appointments
                .Where(x => x.UserName == request.UserName)
                .Select(x => x.DateId);
             
            return await _context.AppointmentDates.AsNoTracking()
                       .Where(x =>
                           x.Date > DateTime.Now &&
                           x.Date >= x.Type.Start &&
                           x.Date <= x.Type.End &&
                           x.TypeId == appointmentType.Id &&
                           !registeredDatesQuery.Contains(x.Id) &&
                           !reservedDatesQuery.Contains(x.Id))
                       .OrderBy(x=> x.Date)
                       .ProjectTo<AppointmentDateDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }

        private async Task<AppointmentType> GetTypeAsync(string slug, bool isPublicRequest, CancellationToken ct)
        {
            var type = await _context.AppointmentTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug, ct);
         
            if (type is null)
            {
                throw new NotFoundException();
            }

            if (isPublicRequest && !type.IsPublic)
            {
                throw new UnauthorizedAccessException();
            }

            return type;
        }
    }


}
