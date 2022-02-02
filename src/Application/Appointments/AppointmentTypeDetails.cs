namespace SkorinosGimnazija.Application.Appointments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AppointmentTypeDetails
{
    public record Query(int Id) : IRequest<AppointmentTypeDto>;

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
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}