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

    public class PublicMenuList
    {
        public class Handler : IRequestHandler<Query, List<PublicMenuDto>>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<PublicMenuDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var (domain, language) = request;
                 
                return await _context.Menus
                    .Where(x => x.Category.Language.Slug == language && x.Domain.Slug == domain && x.IsPublished)
                    .OrderBy(x => x.Order)
                    .ProjectTo<PublicMenuDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }

        public record Query(string Domain, string Language) : IRequest<List<PublicMenuDto>>;
    }
}