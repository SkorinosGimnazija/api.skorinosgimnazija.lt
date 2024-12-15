namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class ObservationTypeList
{
    public record Query : IRequest<List<ObservationTypeDto>>;

    public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Query, List<ObservationTypeDto>>
    {
        public async Task<List<ObservationTypeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.ObservationTypes
                       .AsNoTracking()
                       .ProjectTo<ObservationTypeDto>(mapper.ConfigurationProvider)
                       .OrderByDescending(x => x.Id)
                       .ToListAsync(cancellationToken);
        }
    }
}