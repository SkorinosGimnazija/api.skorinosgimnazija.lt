namespace SkorinosGimnazija.Application.Appointments;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities.Appointments;
using Domain.Entities.Identity;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Validators;
using ValidationException = Common.Exceptions.ValidationException;

public static class AppointmentDateCreate
{
    public record Command(AppointmentDateCreateDto AppointmentDate) : IRequest<AppointmentDateDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.AppointmentDate).NotNull();
        }
    }

    public class Handler : IRequestHandler<Command, AppointmentDateDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<AppointmentDateDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.AppointmentDates.Add(_mapper.Map<AppointmentDate>(request.AppointmentDate)).Entity;

            await _context.SaveChangesAsync();

            return _mapper.Map<AppointmentDateDto>(entity);
        }

    }
}