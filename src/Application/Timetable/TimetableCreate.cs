namespace SkorinosGimnazija.Application.Timetable;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities.Timetable;
using Dtos;
using FluentValidation;
using MediatR;
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