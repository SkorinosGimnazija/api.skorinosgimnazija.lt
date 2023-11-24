namespace SkorinosGimnazija.Application.School;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class ClasstimeDetails
{
    public record Query(int Id) : IRequest<ClasstimeDto>;

    public class Handler : IRequestHandler<Query, ClasstimeDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ClasstimeDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.Classtimes
                             .AsNoTracking()
                             .ProjectTo<ClasstimeDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}