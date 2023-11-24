namespace SkorinosGimnazija.Application.Timetable;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class TimetableDetails
{
    public record Query(int Id) : IRequest<TimetableDto>;

    public class Handler : IRequestHandler<Query, TimetableDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TimetableDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.Timetable
                             .AsNoTracking()
                             .ProjectTo<TimetableDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}