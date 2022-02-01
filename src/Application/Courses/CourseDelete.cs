namespace SkorinosGimnazija.Application.Banners;

using System.Diagnostics.CodeAnalysis;
using Common.Exceptions;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class CourseDelete
{
    public record Command(int Id) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public Handler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            if (!_currentUser.IsOwnerOrAdmin(entity.UserId))
            {
                throw new UnauthorizedAccessException();
            }

            _context.Courses.Remove(entity);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}