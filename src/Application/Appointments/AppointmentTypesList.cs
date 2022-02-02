namespace SkorinosGimnazija.Application.Appointments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AppointmentTypesList
{
    public record Query() : IRequest<List<AppointmentTypeDto>>;

    public class Handler : IRequestHandler<Query, List<AppointmentTypeDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AppointmentTypeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.AppointmentTypes
                       .AsNoTracking()
                       .ProjectTo<AppointmentTypeDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}