﻿namespace Application.Menus
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
        public record Query(string Language) : IRequest<List<MenuDto>>;

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
                    .Where(x => x.IsPublished && x.Category.Language.Slug == request.Language)
                    .OrderBy(x => x.Order)
                    .ProjectTo<MenuDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}