namespace SkorinosGimnazija.Application.School;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class ClassroomDetails
{
    public record Query(int Id) : IRequest<ClassroomDto>;

    public class Handler : IRequestHandler<Query, ClassroomDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ClassroomDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.Classrooms
                             .AsNoTracking()
                             .ProjectTo<ClassroomDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}