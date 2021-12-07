namespace SkorinosGimnazija.Application.Menus;

using System.Diagnostics.CodeAnalysis;
using Common.Exceptions;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class MenuDelete
{
    public record Command(int Id) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly ISearchClient _searchClient;

        public Handler(IAppDbContext context, ISearchClient searchClient)
        {
            _context = context;
            _searchClient = searchClient;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Menus.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            await _searchClient.RemoveMenuAsync(entity);
            _context.Menus.Remove(entity);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}