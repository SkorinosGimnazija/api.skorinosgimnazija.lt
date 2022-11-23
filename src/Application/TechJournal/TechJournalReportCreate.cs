namespace SkorinosGimnazija.Application.TechJournal;
using AutoMapper;
using FluentValidation;
using MediatR;
using SkorinosGimnazija.Application.TechJournal.Validators;
using SkorinosGimnazija.Application.BullyReports.Dtos;
using SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Domain.Entities.Bullies;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.TechReports;
using Dtos;
using Notifications;

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
        private readonly IPublisher _publisher;
        private readonly IMapper _mapper;

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
            var entity = _context.TechJournalReports.Add(_mapper.Map<TechJournalReport>(request.TechJournalReport)).Entity;

            entity.UserId = _currentUser.UserId;

            await _context.SaveChangesAsync();

            await _publisher.Publish(new TechJournalReportCreatedNotification(entity));

            return _mapper.Map<TechJournalReportDto>(entity);
        }
    }
}
