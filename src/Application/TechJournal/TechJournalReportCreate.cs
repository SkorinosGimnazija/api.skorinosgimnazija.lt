namespace SkorinosGimnazija.Application.TechJournal;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities.TechReports;
using Dtos;
using FluentValidation;
using MediatR;
using Notifications;
using Validators;

public static class TechJournalReportCreate
{
    public record Command(TechJournalReportCreateDto TechJournalReport) : IRequest<TechJournalReportDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.TechJournalReport).NotNull().SetValidator(new TechJournalReportCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, TechJournalReportDto>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        private readonly IPublisher _publisher;

        public Handler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUser, IPublisher publisher)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
            _publisher = publisher;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<TechJournalReportDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.TechJournalReports.Add(_mapper.Map<TechJournalReport>(request.TechJournalReport))
                .Entity;

            entity.UserId = _currentUser.UserId;

            await _context.SaveChangesAsync();

            await _publisher.Publish(new TechJournalReportCreatedNotification(entity));

            return _mapper.Map<TechJournalReportDto>(entity);
        }
    }
}