namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class StudentObservationDetails
{
    public record Query(int Id) : IRequest<StudentObservationDto>;

    public class Handler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUser)
        : IRequestHandler<Query, StudentObservationDto>
    {
        public async Task<StudentObservationDto> Handle(Query request, CancellationToken ct)
        {
            var entity = await context.StudentObservations
                             .ProjectTo<StudentObservationDto>(mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            if (!currentUser.IsOwnerOrSocialManager(entity.Teacher.Id))
            {
                throw new UnauthorizedAccessException();
            }

            return entity;
        }
    }
}