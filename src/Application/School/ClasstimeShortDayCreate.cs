namespace SkorinosGimnazija.Application.School;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities.School;
using Dtos;
using FluentValidation;
using MediatR;
using Validators;

public static class ClasstimeShortDayCreate
{
    public record Command(ClasstimeShortDayCreateDto ClasstimeShortDay) : IRequest<ClasstimeShortDayDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.ClasstimeShortDay).NotNull().SetValidator(new ClasstimeShortDayCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, ClasstimeShortDayDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<ClasstimeShortDayDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.ClasstimeShortDays.Add(_mapper.Map<ClasstimeShortDay>(request.ClasstimeShortDay))
                .Entity;

            await _context.SaveChangesAsync();

            return _mapper.Map<ClasstimeShortDayDto>(entity);
        }
    }
}