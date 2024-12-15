namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class ObservationTypeDetails
{
    public record Query(int Id) : IRequest<ObservationTypeDto>;

    public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Query, ObservationTypeDto>
    {
        public async Task<ObservationTypeDto> Handle(Query request, CancellationToken ct)
        {
            var entity = await context.ObservationTypes
                             .ProjectTo<ObservationTypeDto>(mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}