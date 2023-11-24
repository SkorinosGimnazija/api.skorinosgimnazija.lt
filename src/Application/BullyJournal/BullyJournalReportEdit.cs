namespace SkorinosGimnazija.Application.BullyJournal;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Common.Exceptions;
using Validators;

public static class BullyJournalReportEdit
{
    public record Command(BullyJournalReportEditDto BullyJournalReport) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.BullyJournalReport).NotNull().SetValidator(new BullyJournalReportEditValidator());
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
            var entity = await _context.BullyJournalReports
                             .FirstOrDefaultAsync(x => x.Id == request.BullyJournalReport.Id);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            if (!_currentUser.IsOwnerOrAdmin(entity.UserId))
            {
                throw new UnauthorizedAccessException();
            }

            _mapper.Map(request.BullyJournalReport, entity);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}