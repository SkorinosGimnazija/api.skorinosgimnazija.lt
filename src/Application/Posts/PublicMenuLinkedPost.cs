namespace SkorinosGimnazija.Application.Menus;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Posts.Dtos;

public static class PublicMenuLinkedPost
{
    public record Query(string LanguageSlug, string Path) : IRequest<PostDetailsDto>;

    public class Handler : IRequestHandler<Query, PostDetailsDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PostDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var path = Uri.UnescapeDataString(request.Path);

            var entity = await _context.Menus
                             .AsNoTracking()
                             .ProjectTo<MenuDetailsDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x =>
                                     x.IsPublished &&
                                     x.Path == path &&
                                     x.Language.Slug == request.LanguageSlug,
                                 cancellationToken);

            if (entity?.LinkedPost is null)
            {
                throw new NotFoundException();
            }

            return entity.LinkedPost;
        }
    }
}