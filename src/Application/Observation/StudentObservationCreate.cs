namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using Common.Interfaces;
using Domain.Entities.Observation;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Validators;

public static class StudentObservationCreate
{
    public record Command(StudentObservationCreateDto StudentObservation) : IRequest<StudentObservationDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.StudentObservation).NotNull().SetValidator(new StudentObservationCreateValidator());
        }
    }

    public class Handler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUser) : IRequestHandler<Command, StudentObservationDto>
    {
        public async Task<StudentObservationDto> Handle(Command request, CancellationToken ct)
        {
            var entity = context.StudentObservations
                .Add(mapper.Map<StudentObservation>(request.StudentObservation)).Entity;

            entity.TeacherId = currentUser.UserId;
            entity.Types = await context.ObservationTypes
                                .Where(x => request.StudentObservation.TypeIds.Contains(x.Id))
                                .ToListAsync(ct);

            await context.SaveChangesAsync(ct);

            return mapper.Map<StudentObservationDto>(entity);
        }
    }
}