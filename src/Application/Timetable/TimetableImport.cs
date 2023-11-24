namespace SkorinosGimnazija.Application.Timetable;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities.Timetable;
using Dtos;
using FluentValidation;
using MediatR;
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