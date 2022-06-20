namespace SkorinosGimnazija.Application.Accomplishments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AccomplishmentDetails
{
    public record Query(int Id) : IRequest<AccomplishmentDetailsDto>;

    public class Handler : IRequestHandler<Query, AccomplishmentDetailsDto>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUser)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<AccomplishmentDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.Accomplishments
                             .AsNoTracking()
                             .ProjectTo<AccomplishmentDetailsDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            if (!_currentUser.IsOwnerOrAdmin(entity.UserId))
            {
                throw new UnauthorizedAccessException();
            }

            return entity;
        }
    }
}