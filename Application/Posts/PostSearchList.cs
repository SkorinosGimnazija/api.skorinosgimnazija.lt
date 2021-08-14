namespace Application.Posts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
using Application.Posts.Dtos;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Domain.CMS;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class PostSearchList
    {
        public record Query(string SearchText) : IRequest<List<PostDto>>;

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
                    .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                    .Where(x => EF.Functions.ToTsVector("lithuanian", x.Title).Matches(request.SearchText) ||
                                EF.Functions.ILike(x.Title, $"%{request.SearchText}%"))
                    .ToListAsync(cancellationToken);
            }
        }
    }
}