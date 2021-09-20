namespace Application.Posts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Dtos;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Dtos;
    using Extensions;
    using Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class PostSearchList
    {
        public record Query(string SearchText, PaginationDto Pagination) : IRequest<List<PostDto>>;

        public class Handler : IRequestHandler<Query, List<PostDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly ISearchClient _search;

            public Handler(DataContext context, ISearchClient search, IMapper mapper)
            {
                _context = context;
                _search = search;
                _mapper = mapper;
            }

            public async Task<List<PostDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                await _search.Search(request.SearchText);

                return await _context.Posts
                    .AsNoTracking()
                    .Where(x => EF.Functions.ToTsVector("lithuanian", x.Title).Matches(request.SearchText) ||
                                EF.Functions.ILike(x.Title, $"%{request.SearchText}%"))
                    .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                    .OrderByDescending(x => x.PublishDate)
                    .Paginate(request.Pagination)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}