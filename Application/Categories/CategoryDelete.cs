namespace Application.Posts
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class CategoryDelete
    {
        public record Command(int Id) : IRequest<bool>;
        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (entity == null)
                {
                    return false;
                }

                _context.Categories.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

    }
}