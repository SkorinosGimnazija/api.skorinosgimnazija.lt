namespace SkorinosGimnazija.Application.TechJournal;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Validators;

public static class TechJournalReportEdit
{
    public record Command(TechJournalReportEditDto TechJournalReport) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.TechJournalReport).NotNull().SetValidator(new TechJournalReportEditValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUser)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.TechJournalReports
                             .FirstOrDefaultAsync(x => x.Id == request.TechJournalReport.Id);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            if (!_currentUser.IsOwnerOrAdmin(entity.UserId))
            {
                throw new UnauthorizedAccessException();
            }

            _mapper.Map(request.TechJournalReport, entity);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}