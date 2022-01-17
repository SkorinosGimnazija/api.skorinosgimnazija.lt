namespace SkorinosGimnazija.Application.Appointments;
using AutoMapper;
using FluentValidation;
using MediatR;
using SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Appointments;
using Dtos;
using ParentAppointments.Dtos;
using Validators;
using SkorinosGimnazija.Application.BullyReports.Dtos;
using SkorinosGimnazija.Application.BullyReports.Events;
using SkorinosGimnazija.Domain.Entities.Bullies;

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

        public Handler(IAppDbContext context,  IMapper mapper)
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
