namespace SkorinosGimnazija.Application.BullyReports;
using AutoMapper;
using MediatR;
using SkorinosGimnazija.Application.BullyReports.Dtos;
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

public static class BullyReportDetails
{
    public record Query(int Id) : IRequest<BullyReportDto>;

    public class Handler : IRequestHandler<Query, BullyReportDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BullyReportDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.BullyReports
                             .AsNoTracking()
                             .ProjectTo<BullyReportDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}