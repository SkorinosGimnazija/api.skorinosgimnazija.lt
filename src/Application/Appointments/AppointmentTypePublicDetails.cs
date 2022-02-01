namespace SkorinosGimnazija.Application.Appointments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ParentAppointments.Dtos;

public static class AppointmentTypePublicDetails
{
    public record Query(string Slug) : IRequest<AppointmentTypeDto>;

    public class Handler : IRequestHandler<Query, AppointmentTypeDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppointmentTypeDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.AppointmentTypes
                             .AsNoTracking()
                             .ProjectTo<AppointmentTypeDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x =>
                                     x.Slug == request.Slug &&
                                     x.IsPublic,
                                 cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}