namespace Application.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Dtos;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Dtos;
    using Extensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class PublicPostSearchList
    {
        public record Query(string Language, string SearchText, PaginationDto Pagination) : IRequest<List<PostDto>>;

        public class Handler : IRequestHandler<Query, List<PostDto>>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<PostDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Posts
                    .AsNoTracking()
                    .Where(x => x.IsPublished && x.PublishDate <= DateTime.Now &&
                                x.Category.Language.Slug == request.Language &&
                                EF.Functions.ToTsVector("lithuanian", x.Title).Matches(request.SearchText) ||
                                EF.Functions.ILike(x.Title, $"%{request.SearchText}%"))
                    .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                    .OrderByDescending(x => x.PublishDate)
                    .Paginate(request.Pagination)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}