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

    public class PublicPostList
    {
        public record Query(string Domain, string Language, int PageNr) : IRequest<List<PublicPostDto>>;
        public class Handler : IRequestHandler<Query, List<PublicPostDto>>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<PublicPostDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                const int PostsPerPage = 5;
                var (domain, language, pageNr) = request;
                var postsToSkip = Math.Max(pageNr - 1, 0) * PostsPerPage;
                 
                return await _context.Posts
                    .Where(
                        x => x.IsPublished && x.Category.ShowOnHomePage && x.PublishDate <= DateTime.Now
                             && x.Category.Language.Slug == language && x.Domain.Slug == domain)
                    .OrderByDescending(x => x.IsFeatured)
                    .ThenByDescending(x => x.PublishDate)
                    .Skip(postsToSkip)
                    .Take(PostsPerPage)
                    .ProjectTo<PublicPostDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }

    }
}