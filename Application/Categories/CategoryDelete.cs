namespace Application.Categories;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Diagnostics.CodeAnalysis;

public static class CategoryDelete
{
    public record Command(int Id) : IRequest<bool>;

    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<bool> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity == null)
            {
                return false;
            }

            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}