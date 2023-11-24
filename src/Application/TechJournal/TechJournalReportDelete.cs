namespace SkorinosGimnazija.Application.TechJournal;

using System.Diagnostics.CodeAnalysis;
using Common.Exceptions;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class TechJournalReportDelete
{
    public record Command(int Id) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public Handler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUser = currentUserService;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.TechJournalReports.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            if (!_currentUser.IsAdmin() && _currentUser.UserId != entity.UserId)
            {
                throw new UnauthorizedAccessException();
            }

            _context.TechJournalReports.Remove(entity);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}