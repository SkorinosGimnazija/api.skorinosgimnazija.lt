namespace Application.Posts
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.CMS;
    using MediatR;
    using Persistence;

    public class Edit
    {
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var post = await _context.Posts.FindAsync(new object[] { request.Post.Id }, cancellationToken);
                //if (post == null)
                //{

                //}

                _mapper.Map(request.Post, post);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

        public record Command(Post Post) : IRequest;
    }
}