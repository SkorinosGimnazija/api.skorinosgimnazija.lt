namespace SkorinosGimnazija.Application.Banners;
using AutoMapper;
using MediatR;
using SkorinosGimnazija.Application.Banners.Dtos;

using SkorinosGimnazija.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public static class PublicBannerList
{
    public record Query(string LanguageSlug) : IRequest<List<BannerDto>>;

    public class Handler : IRequestHandler<Query, List<BannerDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BannerDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Banners
                       .AsNoTracking()
                       .Where(x =>
                           x.IsPublished &&
                           x.Language.Slug == request.LanguageSlug)
                       .ProjectTo<BannerDto>(_mapper.ConfigurationProvider)
                       .OrderBy(x => x.Order)
                       .ToListAsync(cancellationToken);
        }
    }
}
