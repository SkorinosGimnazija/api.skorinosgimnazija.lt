namespace Application.Posts
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.CMS;
    using MediatR;
    using Persistence;

    public class Details
    {
        public class Handler : IRequestHandler<Query, Post>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Post> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Posts.FindAsync(new object[] { request.Id }, cancellationToken);
            }
        }

        public record Query (int Id) : IRequest<Post>;
    }
}