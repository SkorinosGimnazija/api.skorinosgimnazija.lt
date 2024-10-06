namespace SkorinosGimnazija.Application.Menus;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class PublicMenuList
{
    public record Query(string Language) : IRequest<List<MenuPublicDto>>;

    public class Handler : IRequestHandler<Query, List<MenuPublicDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MenuPublicDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Menus
                       .AsNoTracking()
                       .Where(x =>
                           x.IsPublished &&
                           x.Language.Slug == request.Language &&
                           x.MenuLocation.Slug != "off")
                       .OrderBy(x => x.Order)
                       .ProjectTo<MenuPublicDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}