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

public static class AppointmentReservedDateCreate
{
    public record Command(AppointmentReservedDateCreateDto AppointmentDate) : IRequest<AppointmentReservedDateDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.AppointmentDate).NotNull();
        }
    }

    public class Handler : IRequestHandler<Command, AppointmentReservedDateDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<AppointmentReservedDateDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.AppointmentReservedDates.Add(_mapper.Map<AppointmentReservedDate>(request.AppointmentDate)).Entity;

            await _context.SaveChangesAsync();

            return _mapper.Map<AppointmentReservedDateDto>(entity);
        }

    }
}