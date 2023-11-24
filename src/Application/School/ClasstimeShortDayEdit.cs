﻿namespace SkorinosGimnazija.Application.School;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Validators;

public static class ClasstimeShortDayEdit
{
    public record Command(ClasstimeShortDayEditDto ClasstimeShortDay) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.ClasstimeShortDay).NotNull().SetValidator(new ClasstimeShortDayEditValidator());
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
            var entity =
                await _context.ClasstimeShortDays.FirstOrDefaultAsync(x => x.Id == request.ClasstimeShortDay.Id);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(request.ClasstimeShortDay, entity);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}