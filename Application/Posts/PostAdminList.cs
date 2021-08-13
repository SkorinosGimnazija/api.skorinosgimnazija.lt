namespace Application.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Domain.CMS;
    using Dtos;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class PostAdminList
    {
        public record Query(int PageNr) : IRequest<List<Post>>;
        public class Handler : IRequestHandler<Query, List<Post>>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<Post>> Handle(Query request, CancellationToken cancellationToken)
            {
                const int PostsPerPage = 10;
                var page = Math.Max(request.PageNr - 1, 0);
                var postsToSkip = page * PostsPerPage;
                  
                return await _context.Posts 
                    .ProjectTo<Post>(_mapper.ConfigurationProvider)
                    .OrderBy(x => x.IsFeatured)
                    .ThenBy(x => x.PublishDate)
                    .Skip(postsToSkip)
                    .Take(PostsPerPage)
                    .ToListAsync(cancellationToken);
            }
        }

    }
}