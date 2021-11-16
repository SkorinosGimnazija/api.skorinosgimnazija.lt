namespace Application.Posts;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Dtos;
using Core.Extensions;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

public static class PublicPostList
{
    public record Query(string LanguageSlug, PaginationDto Pagination) : IRequest<List<PostDto>>;

    public class Handler : IRequestHandler<Query, List<PostDto>>
    {
        private readonly DataContext _context;

        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PostDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Posts
                .AsNoTracking()
                .Where(x =>
                    x.IsPublished &&
                    x.Category.ShowOnHomePage &&
                    x.PublishDate <= DateTime.UtcNow &&
                    x.Category.Language.Slug == request.LanguageSlug)
                .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(x => x.IsFeatured)
                .ThenByDescending(x => x.PublishDate)
                .Paginate(request.Pagination)
                .ToListAsync(cancellationToken);
        }
    }
}