namespace Application.Menus
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Domain.CMS;
    using Dtos;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class MenuAdminList
    {
        public class Handler : IRequestHandler<Query, List<Menu>>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<Menu>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Menus
                    .OrderBy(x => x.Order)
                    .ProjectTo<Menu>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }

        public record Query() : IRequest<List<Menu>>;
    }
}