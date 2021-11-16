namespace Application.Posts;

using Core.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

public static class PostDelete
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

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<bool> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Posts.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity is null)
            {
                return false;
            }

            await _search.RemovePost(entity.Id);
            _fileManager.DeleteAllFiles(entity.Id);
            
            _context.Posts.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}