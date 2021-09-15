namespace Application.Posts
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class PostDelete
    {
        public record Command(int Id) : IRequest<bool>;
        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly DataContext _context;
            private readonly ISearchClient _search;

            public Handler(DataContext context, ISearchClient search)
            {
                _context = context;
                _search = search;
            }
              
            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _context.Posts.FirstOrDefaultAsync(x=> x.Id == request.Id, cancellationToken);
                if (entity is null)
                {
                    return false;
                }
            
                _context.Posts.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                await _search.RemovePost(entity.Id);

                return true;
            }
        }

    }
}