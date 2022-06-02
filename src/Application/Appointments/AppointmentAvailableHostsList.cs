namespace SkorinosGimnazija.Application.Appointments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Employees.Dtos;
using SkorinosGimnazija.Domain.Entities.Appointments;

public static class AppointmentAvailableHostsList
{
    public record Query(string TypeSlug, bool IsPublic) : IRequest<List<AppointmentHostDto>>;

    public class Handler : IRequestHandler<Query, List<AppointmentHostDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public Handler(IAppDbContext context, IMapper mapper, IEmployeeService employeeService)
        {
            _context = context;
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<List<AppointmentHostDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var appointmentType = await GetTypeAsync(request.TypeSlug, request.IsPublic, cancellationToken);

            var teachers = await _employeeService.GetTeachersAsync(cancellationToken);

            var exclusiveHosts = await _context.AppointmentExclusiveHosts.AsNoTracking()
                                     .Where(x => x.TypeId == appointmentType.Id)
                                     .Select(x => x.UserName)
                                     .ToListAsync(cancellationToken);

            return _mapper.Map<List<AppointmentHostDto>>(exclusiveHosts.Any()
                                                             ? teachers.IntersectBy(exclusiveHosts, x => x.Id)
                                                             : teachers);
        }

        private async Task<AppointmentType> GetTypeAsync(string slug, bool isPublicRequest, CancellationToken ct)
        {
            var type = await _context.AppointmentTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug, ct);

            if (type is null)
            {
                throw new NotFoundException();
            }

            if (isPublicRequest != type.IsPublic)
            {
                throw new UnauthorizedAccessException();
            }

            return type;
        }
    }
}