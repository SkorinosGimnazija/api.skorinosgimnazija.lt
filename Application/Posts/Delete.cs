namespace Application.Posts
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Persistence;

    public class Delete
    {
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var post = await _context.Posts.FindAsync(new object[] { request.Id }, cancellationToken);

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

        public record Command(int Id) : IRequest;
    }
}