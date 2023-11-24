namespace SkorinosGimnazija.Application.Appointments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AppointmentHostsList
{
    public record Query(int TypeId) : IRequest<List<AppointmentExclusiveHostDto>>;

    public class Handler : IRequestHandler<Query, List<AppointmentExclusiveHostDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AppointmentExclusiveHostDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.AppointmentExclusiveHosts.AsNoTracking()
                       .Where(x => x.TypeId == request.TypeId)
                       .ProjectTo<AppointmentExclusiveHostDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}