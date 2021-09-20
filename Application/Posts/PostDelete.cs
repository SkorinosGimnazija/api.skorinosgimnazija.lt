namespace Application.Posts
{
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class PostDelete
    {
        public record Command(int Id) : IRequest<bool>;

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly DataContext _context;
            private readonly IFileManager _fileManager;
            private readonly ISearchClient _search;

            public Handler(DataContext context, ISearchClient search, IFileManager fileManager)
            {
                _context = context;
                _search = search;
                _fileManager = fileManager;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _context.Posts.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (entity is null)
                {
                    return false;
                }

                _context.Posts.Remove(entity);
                await _search.RemovePost(entity.Id);
                await _fileManager.DeleteAllFilesAsync(entity.Id);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}