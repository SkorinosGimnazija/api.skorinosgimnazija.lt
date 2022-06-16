namespace SkorinosGimnazija.Application.Banners;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class PublicBannerList
{
    public record Query(string LanguageSlug) : IRequest<List<BannerPublicDto>>;

    public class Handler : IRequestHandler<Query, List<BannerPublicDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BannerPublicDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Banners
                       .AsNoTracking()
                       .Where(x =>
                           x.IsPublished &&
                           x.Language.Slug == request.LanguageSlug)
                       .OrderBy(x => x.Order)
                       .ProjectTo<BannerPublicDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}