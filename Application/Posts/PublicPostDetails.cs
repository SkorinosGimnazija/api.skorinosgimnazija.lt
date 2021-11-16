namespace Application.Posts;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

public static class PublicPostDetails
{
    public record Query(int Id, string CategorySlug) : IRequest<PostDetailsDto?>;

    public class Handler : IRequestHandler<Query, PostDetailsDto?>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PostDetailsDto?> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Posts
                .AsNoTracking()
                .ProjectTo<PostDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x =>
                        x.Id == request.Id &&
                        x.Category.Slug == request.CategorySlug &&
                        x.IsPublished &&
                        x.PublishDate <= DateTime.UtcNow,
                    cancellationToken);
        }
    }
}