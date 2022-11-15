namespace SkorinosGimnazija.Application.TechJournal;
using AutoMapper;
using MediatR;
using SkorinosGimnazija.Application.TechJournal.Dtos;
using SkorinosGimnazija.Application.Common.Exceptions;

using SkorinosGimnazija.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public static class TechJournalReportDetails
{
    public record Query(int Id) : IRequest<TechJournalReportDto>;

    public class Handler : IRequestHandler<Query, TechJournalReportDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TechJournalReportDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.TechJournalReports
                             .AsNoTracking()
                             .ProjectTo<TechJournalReportDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}
