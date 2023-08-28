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

public static class TimetableCreate
{
    public record Command(TimetableCreateDto Timetable) : IRequest<TimetableDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Timetable).NotNull().SetValidator(new TimetableCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, TimetableDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<TimetableDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.Timetable.Add(_mapper.Map<Timetable>(request.Timetable)).Entity;

            await _context.SaveChangesAsync();

            return _mapper.Map<TimetableDto>(entity);
        }
    }
}
