namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class ObservationTargetDetails
{
    public record Query(int Id) : IRequest<ObservationTargetDto>;

    public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Query, ObservationTargetDto>
    {
        public async Task<ObservationTargetDto> Handle(Query request, CancellationToken ct)
        {
            var entity = await context.ObservationTargets
                             .ProjectTo<ObservationTargetDto>(mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}