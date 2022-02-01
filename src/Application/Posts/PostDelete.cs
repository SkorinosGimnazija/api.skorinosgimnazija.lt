namespace SkorinosGimnazija.Application.Posts;

using System.Diagnostics.CodeAnalysis;
using Common.Exceptions;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class PostDelete
{
    public record Command(int Id) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly IMediaManager _mediaManager;
        private readonly ISearchClient _searchClient;

        public Handler(IAppDbContext context, ISearchClient searchClient, IMediaManager mediaManager)
        {
            _context = context;
            _searchClient = searchClient;
            _mediaManager = mediaManager;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Posts.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            await _searchClient.RemovePostAsync(entity);
            _mediaManager.DeleteFiles(entity.Files);
            _mediaManager.DeleteFiles(entity.Images);
            _mediaManager.DeleteFiles(entity.FeaturedImage);

            _context.Posts.Remove(entity);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}