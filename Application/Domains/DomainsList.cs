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

    public class DomainsList
    {
        public record Query() : IRequest<List<Domain>>;

        public class Handler : IRequestHandler<Query, List<Domain>>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<Domain>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Domains
                    .ProjectTo<Domain>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }

    }
}