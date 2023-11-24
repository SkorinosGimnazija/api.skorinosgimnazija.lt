namespace SkorinosGimnazija.Application.Appointments;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities.Appointments;
using Dtos;
using FluentValidation;
using MediatR;
using Validators;

public static class AppointmentHostCreate
{
    public record Command
        (AppointmentExclusiveHostCreateDto AppointmentExclusiveHost) : IRequest<AppointmentExclusiveHostDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator(IEmployeeService employeeService)
        {
            RuleFor(v => v.AppointmentExclusiveHost)
                .NotNull()
                .SetValidator(new AppointmentHostCreateValidator(employeeService));
        }
    }

    public class Handler : IRequestHandler<Command, AppointmentExclusiveHostDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<AppointmentExclusiveHostDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.AppointmentExclusiveHosts
                .Add(_mapper.Map<AppointmentExclusiveHost>(request.AppointmentExclusiveHost))
                .Entity;

            await _context.SaveChangesAsync();

            return _mapper.Map<AppointmentExclusiveHostDto>(entity);
        }
    }
}