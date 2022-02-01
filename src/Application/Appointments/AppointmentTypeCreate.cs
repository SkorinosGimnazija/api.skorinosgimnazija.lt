namespace SkorinosGimnazija.Application.Appointments;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities.Appointments;
using Dtos;
using FluentValidation;
using MediatR;
using ParentAppointments.Dtos;
using Validators;

public static class AppointmentTypeCreate
{
    public record Command(AppointmentTypeCreateDto AppointmentType) : IRequest<AppointmentTypeDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.AppointmentType).NotNull().SetValidator(new AppointmentTypeCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, AppointmentTypeDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<AppointmentTypeDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.AppointmentTypes.Add(_mapper.Map<AppointmentType>(request.AppointmentType)).Entity;

            await _context.SaveChangesAsync();

            return _mapper.Map<AppointmentTypeDto>(entity);
        }
    }
}