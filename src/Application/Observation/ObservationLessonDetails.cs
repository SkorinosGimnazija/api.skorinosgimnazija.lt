namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class ObservationLessonDetails
{
    public record Query(int Id) : IRequest<ObservationLessonDto>;

    public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Query, ObservationLessonDto>
    {
        public async Task<ObservationLessonDto> Handle(Query request, CancellationToken ct)
        {
            var entity = await context.ObservationLessons
                             .ProjectTo<ObservationLessonDto>(mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}