namespace SkorinosGimnazija.Application.Menus;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class MenuLocationsList
{
    public record Query() : IRequest<List<MenuLocationDto>>;

    public class Handler : IRequestHandler<Query, List<MenuLocationDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MenuLocationDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.MenuLocations
                       .AsNoTracking()
                       .ProjectTo<MenuLocationDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}