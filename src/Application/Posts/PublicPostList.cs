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

public static class PublicPostList
{
    public record Query(string LanguageSlug, PaginationDto Pagination) : IRequest<List<PostDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
        }
    }

    public class Handler : IRequestHandler<Query, List<PostDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PostDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Posts
                       .AsNoTracking()
                       .Where(x =>
                           x.IsPublished &&
                           x.ShowInFeed &&
                           x.PublishedAt <= DateTime.UtcNow &&
                           x.Language.Slug == request.LanguageSlug)
                       .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                       .OrderByDescending(x => x.IsFeatured)
                       .ThenByDescending(x => x.PublishedAt)
                       .Paginate(request.Pagination)
                       .ToListAsync(cancellationToken);
        }
    }
}