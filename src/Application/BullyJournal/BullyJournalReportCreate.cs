namespace SkorinosGimnazija.Application.BullyJournal;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BullyReports.Dtos;
using Common.Interfaces;
using Domain.Entities.Bullies;
using FluentValidation;
using MediatR;
using Validators;

public static class BullyJournalReportCreate
{
    public record Command(BullyJournalReportCreateDto BullyJournalReport) : IRequest<BullyJournalReportDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.BullyJournalReport).NotNull().SetValidator(new BullyJournalReportCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, BullyJournalReportDto>
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
        public async Task<BullyJournalReportDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.BullyJournalReports.Add(_mapper.Map<BullyJournalReport>(request.BullyJournalReport)).Entity;

            entity.UserId = _currentUser.UserId;

            await _context.SaveChangesAsync();

            return _mapper.Map<BullyJournalReportDto>(entity);
        }
    }
}
