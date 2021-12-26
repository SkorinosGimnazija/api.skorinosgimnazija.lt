namespace SkorinosGimnazija.Application.Posts;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Common.Interfaces;
using Common.Pagination;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class PostList
{
    public record Query(PaginationDto Pagination) : IRequest<PaginatedList<PostDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
        }
    }

    public class Handler : IRequestHandler<Query, PaginatedList<PostDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<PostDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Posts
                       .AsNoTracking()
                       .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                       .OrderByDescending(x => x.PublishedAt)
                       //.OrderByDescending(x => x.IsFeatured)
                       //.ThenByDescending(x => x.PublishDate)
                       .ToPaginatedListAsync(request.Pagination, cancellationToken);
        }
    }
}