namespace SkorinosGimnazija.Application.Appointments;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Validators;

public static class AppointmentTypeEdit
{
    public record Command(AppointmentTypeEditDto AppointmentType) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.AppointmentType).NotNull().SetValidator(new AppointmentTypeEditValidator());
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
            var entity = await _context.AppointmentTypes.FirstOrDefaultAsync(x => x.Id == request.AppointmentType.Id);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(request.AppointmentType, entity);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}