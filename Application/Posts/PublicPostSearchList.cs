namespace Application.Posts
{
    using System;
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

    public class PublicPostSearchList
    {
        public record Query(string Domain, string Language, string SearchText) : IRequest<List<PublicPostDto>>;

        public class Handler : IRequestHandler<Query, List<PublicPostDto>>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<PublicPostDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var (domain, language, searchText) = request;

                return await _context.Posts
                    .Where(x => x.IsPublished && x.PublishDate <= DateTime.Now && x.Domain.Slug == domain &&
                                x.Category.Language.Slug == language &&
                                EF.Functions.ToTsVector("lithuanian", x.Title).Matches(searchText) ||
                                EF.Functions.ILike(x.Title, $"%{searchText}%"))
                    .ProjectTo<PublicPostDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}