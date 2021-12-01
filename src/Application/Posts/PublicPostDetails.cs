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
    public record Query(int Id) : IRequest<PostDetailsDto>;

    public class Handler : IRequestHandler<Query, PostDetailsDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PostDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.Posts
                             .AsNoTracking()
                             .ProjectTo<PostDetailsDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x =>
                                     x.Id == request.Id &&
                                     x.IsPublished &&
                                     x.PublishDate <= DateTime.UtcNow,
                                 cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}