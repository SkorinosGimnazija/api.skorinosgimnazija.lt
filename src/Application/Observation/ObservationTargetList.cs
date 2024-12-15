namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class ObservationTargetList
{
    public record Query(bool EnabledOnly) : IRequest<List<ObservationTargetDto>>;

    public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Query, List<ObservationTargetDto>>
    {
        public async Task<List<ObservationTargetDto>> Handle(Query request, CancellationToken ct)
        {
            var query = context.ObservationTargets.AsNoTracking();

            if (request.EnabledOnly)
            {
                query = query.Where(x => x.Enabled);
            }

            return await query
                       .ProjectTo<ObservationTargetDto>(mapper.ConfigurationProvider)
                       .OrderByDescending(x => x.Id)
                       .ToListAsync(ct);
        }
    }
}