namespace SkorinosGimnazija.Application.Menus;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class MenuDetails
{
    public record Query(int Id) : IRequest<MenuDetailsDto>;

    public class Handler : IRequestHandler<Query, MenuDetailsDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MenuDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.Menus
                             .AsNoTracking()
                             .ProjectTo<MenuDetailsDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}