namespace SkorinosGimnazija.Application.Banners;
using AutoMapper;
using MediatR;
using SkorinosGimnazija.Application.Banners.Dtos;
using SkorinosGimnazija.Application.Common.Exceptions;
using SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Application.Menus.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public static class BannerDetails
{
    public record Query(int Id) : IRequest<BannerDto>;

    public class Handler : IRequestHandler<Query, BannerDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BannerDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.Banners
                             .AsNoTracking()
                             .ProjectTo<BannerDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}
