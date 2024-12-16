namespace SkorinosGimnazija.Application.TechJournal;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notifications;
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
        private readonly IPublisher _publisher;

        public Handler(IAppDbContext context, IMapper mapper, IPublisher publisher, ICurrentUserService currentUser)
        {
            _context = context;
            _mapper = mapper;
            _publisher = publisher;
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

            var oldState = entity.IsFixed;

            _mapper.Map(request.TechJournalReport, entity);

            await _context.SaveChangesAsync();

            await _publisher.Publish(new TechJournalReportPatchedNotification(entity, oldState));

            return Unit.Value;
        }
    }
}