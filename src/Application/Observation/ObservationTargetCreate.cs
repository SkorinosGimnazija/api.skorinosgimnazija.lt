namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using Common.Interfaces;
using Domain.Entities.Observation;
using Dtos;
using FluentValidation;
using MediatR;
using Validators;

public static class ObservationTargetCreate
{
    public record Command(ObservationTargetCreateDto Target) : IRequest<ObservationTargetDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Target).NotNull().SetValidator(new ObservationTargetCreateValidator());
        }
    }

    public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, ObservationTargetDto>
    {
        public async Task<ObservationTargetDto> Handle(Command request, CancellationToken ct)
        {
            var entity = context.ObservationTargets.Add(mapper.Map<ObservationTarget>(request.Target)).Entity;

            await context.SaveChangesAsync(ct);

            return mapper.Map<ObservationTargetDto>(entity);
        }
    }
}