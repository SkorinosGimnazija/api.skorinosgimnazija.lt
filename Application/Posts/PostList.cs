namespace Application.Posts;

using Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dtos;
using Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

public static class PostList
{
    public record Query(PaginationDto Pagination) : IRequest<List<PostDto>>;

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
                .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(x => x.PublishDate)
                .Paginate(request.Pagination)
                .ToListAsync(cancellationToken);
        }
    }
}