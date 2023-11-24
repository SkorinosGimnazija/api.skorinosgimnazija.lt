namespace SkorinosGimnazija.Application.School;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities.School;
using Dtos;
using FluentValidation;
using MediatR;
using Validators;

public static class ClassroomCreate
{
    public record Command(ClassroomCreateDto Classroom) : IRequest<ClassroomDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Classroom).NotNull().SetValidator(new ClassroomCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, ClassroomDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<ClassroomDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.Classrooms.Add(_mapper.Map<Classroom>(request.Classroom)).Entity;

            await _context.SaveChangesAsync();

            return _mapper.Map<ClassroomDto>(entity);
        }
    }
}