namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class ObservationLessonList
{
    public record Query : IRequest<List<ObservationLessonDto>>;

    public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Query, List<ObservationLessonDto>>
    {
        public async Task<List<ObservationLessonDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.ObservationLessons
                       .AsNoTracking()
                       .ProjectTo<ObservationLessonDto>(mapper.ConfigurationProvider)
                       .OrderBy(x => x.Name)
                       .ToListAsync(cancellationToken);
        }
    }
}