namespace SkorinosGimnazija.Application.Timetable;
using AutoMapper;
using FluentValidation;
using MediatR;
using SkorinosGimnazija.Application.BullyJournal.Validators;
using SkorinosGimnazija.Application.BullyReports.Dtos;
using SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Domain.Entities.Bullies;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Timetable;
using Dtos;
using Validators;

public static class TimetableImport
{
    public record Command(TimetableImportDto Timetable) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Timetable).NotNull().SetValidator(new TimetableImportValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var list = _mapper.Map<List<Timetable>>(request.Timetable.TimetableList);

            await _context.Timetable.AddRangeAsync(list);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
