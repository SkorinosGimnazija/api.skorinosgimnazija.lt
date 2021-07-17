namespace Application.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Dtos;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class PublicList
    {
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
                const int PostsPerPage = 5;

                var lang = request.Language.ToLowerInvariant();
                var pageNr = request.PageNr - 1;
                var postsToSkip = pageNr * PostsPerPage;

                return await _context.Posts
                           .Where(
                               x => x.IsPublished && x.PublishDate <= DateTime.Now && x.Category.ShowOnHomePage
                                    && x.Category.Language.Slug == lang)
                           .OrderBy(x => x.IsFeatured)
                           .ThenBy(x => x.PublishDate)
                           .Skip(postsToSkip)
                            .Take(PostsPerPage)
                           .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                           .ToListAsync(cancellationToken);
            }
        }

        public record Query(string Language, int PageNr) : IRequest<List<PostDto>>;
    }
}