namespace SkorinosGimnazija.Application.Appointments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AppointmentDetails
{
    public record Query(int Id) : IRequest<AppointmentDetailsDto>;

    public class Handler : IRequestHandler<Query, AppointmentDetailsDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppointmentDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.Appointments
                             .AsNoTracking()
                             .ProjectTo<AppointmentDetailsDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}