namespace SkorinosGimnazija.Application.Posts;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class PublicPostDetails
{
    public record Query(int Id) : IRequest<PostPublicDetailsDto>;

    public class Handler : IRequestHandler<Query, PostPublicDetailsDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PostPublicDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.Posts
                             .AsNoTracking()
                             .Where(x => x.IsPublished && x.PublishedAt <= DateTime.UtcNow)
                             .ProjectTo<PostPublicDetailsDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}