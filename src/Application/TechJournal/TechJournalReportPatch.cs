namespace SkorinosGimnazija.Application.TechJournal;
using AutoMapper;
using FluentValidation;
using MediatR;
using SkorinosGimnazija.Application.Common.Exceptions;
using SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Application.TechJournal.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Validators;

public static class TechJournalReportPatch
{
    public record Command(int Id, TechJournalReportPatchDto TechJournalReport) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.TechJournalReport).NotNull().SetValidator(new TechJournalReportPatchValidator());
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
                             .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            if (!_currentUser.IsAdmin() && !_currentUser.IsTechManager())
            {
                throw new UnauthorizedAccessException();
            }

            _mapper.Map(request.TechJournalReport, entity);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}