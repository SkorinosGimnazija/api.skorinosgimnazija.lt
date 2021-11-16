namespace Application.Menus;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Diagnostics.CodeAnalysis;

public static class MenuDelete
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
            var entity = await _context.Menus.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity == null)
            {
                return false;
            }

            _context.Menus.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}