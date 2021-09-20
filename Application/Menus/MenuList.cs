namespace Application.Menus
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Dtos;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class MenuList
    {
        public record Query() : IRequest<List<MenuDto>>;

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
                return await _context.Menus
                    .AsNoTracking()
                    .OrderBy(x => x.Order)
                    .ProjectTo<MenuDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}