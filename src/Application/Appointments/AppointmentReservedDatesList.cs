namespace SkorinosGimnazija.Application.Appointments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AppointmentReservedDatesList
{
    public record Query(string UserName) : IRequest<List<AppointmentReservedDateDto>>;

    public class Handler : IRequestHandler<Query, List<AppointmentReservedDateDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AppointmentReservedDateDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.AppointmentReservedDates.AsNoTracking()
                       .Where(x => x.UserName == request.UserName)
                       .OrderBy(x => x.Date)
                       .ProjectTo<AppointmentReservedDateDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}