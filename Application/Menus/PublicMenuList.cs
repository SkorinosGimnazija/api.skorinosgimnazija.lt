namespace Application.Menus;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

public static class PublicMenuList
{
    public record Query(string Language, string LocationSlug) : IRequest<List<MenuDto>>;

    public class Handler : IRequestHandler<Query, List<MenuDto>>
    {
        private readonly DataContext _context;

        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MenuDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var menus = await _context.Menus
                .AsNoTracking()
                .Where(x =>
                    x.IsPublished &&
                    x.Language.Slug == request.Language &&
                    x.MenuLocation.Slug == request.LocationSlug)
                .OrderBy(x => x.Order)
                .ProjectTo<MenuDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            // can ef populate menus ?
            foreach (var childMenu in menus.Where(x => x.ParentMenuId is not null))
            {
                var parent = menus.Find(x => x.Id == childMenu.ParentMenuId);
                parent?.ChildMenus.Add(childMenu);
            }

            return menus.Where(x => x.ParentMenuId is null).ToList();
        }
    }
}