namespace Application.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.CMS;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class List
    {
        public class Handler : IRequestHandler<Query, List<Post>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<Post>> Handle(Query request, CancellationToken cancellationToken)
            {
                const int PostsPerPage = 5;
                var skip = Math.Max(request.Page, 0) * PostsPerPage;

                return await _context.Posts.OrderBy(x => x.PublishDate)
                           .Skip(skip)
                           .Take(PostsPerPage)
                           .ToListAsync(cancellationToken);
            }
        }

        public record Query(int Page) : IRequest<List<Post>>;
    }
}