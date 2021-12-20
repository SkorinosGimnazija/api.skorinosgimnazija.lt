namespace SkorinosGimnazija.Application.Banners;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Common.Exceptions;

using SkorinosGimnazija.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class CourseDelete
{
    public record Command(int Id) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public Handler(IAppDbContext context,  ICurrentUserService currentUser)
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

            var q0 = entity.UserId;
            var q1 = _currentUser.UserId;
            var q2 = _currentUser.IsAdmin();
            var q3 = _currentUser.IsResourceOwner(entity.UserId);
            var q4 = _currentUser.IsOwnerOrAdmin(entity.UserId);
            
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