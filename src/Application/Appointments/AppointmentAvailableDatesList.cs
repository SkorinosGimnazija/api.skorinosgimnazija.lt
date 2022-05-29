namespace SkorinosGimnazija.Application.Appointments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Domain.Entities.Appointments;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
                           x.TypeId == appointmentType.Id &&
                           !registeredDatesQuery.Contains(x.Id) &&
                           !reservedDatesQuery.Contains(x.Id))
                       .OrderBy(x => x.Date)
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