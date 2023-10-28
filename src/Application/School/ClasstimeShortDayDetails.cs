namespace SkorinosGimnazija.Application.School;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Common.Exceptions;
using SkorinosGimnazija.Application.Common.Interfaces;
using SkorinosGimnazija.Application.School.Dtos;

public static class ClasstimeShortDayDetails
{
    public record Query(int Id) : IRequest<ClasstimeShortDayDto>;

    public class Handler : IRequestHandler<Query, ClasstimeShortDayDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ClasstimeShortDayDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.ClasstimeShortDays
                             .AsNoTracking()
                             .ProjectTo<ClasstimeShortDayDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}