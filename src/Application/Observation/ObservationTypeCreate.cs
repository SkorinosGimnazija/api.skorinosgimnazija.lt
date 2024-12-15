namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using Common.Interfaces;
using Domain.Entities.Observation;
using Dtos;
using FluentValidation;
using MediatR;
using Validators;

public static class ObservationTypeCreate
{
    public record Command(ObservationTypeCreateDto Type) : IRequest<ObservationTypeDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Type).NotNull().SetValidator(new ObservationTypeCreateValidator());
        }
    }

    public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, ObservationTypeDto>
    {
        public async Task<ObservationTypeDto> Handle(Command request, CancellationToken ct)
        {
            var entity = context.ObservationTypes.Add(mapper.Map<ObservationType>(request.Type)).Entity;

            await context.SaveChangesAsync(ct);

            return mapper.Map<ObservationTypeDto>(entity);
        }
    }
}