namespace SkorinosGimnazija.Application.School;
using AutoMapper;
using FluentValidation;
using MediatR;
using SkorinosGimnazija.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.School;
using Dtos;
using Validators;

public static class ClasstimeCreate
{
    public record Command(ClasstimeCreateDto Classtime) : IRequest<ClasstimeDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Classtime).NotNull().SetValidator(new ClasstimeCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, ClasstimeDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<ClasstimeDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.Classtimes.Add(_mapper.Map<Classtime>(request.Classtime)).Entity;

            await _context.SaveChangesAsync();

            return _mapper.Map<ClasstimeDto>(entity);
        }
    }
}
