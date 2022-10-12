namespace SkorinosGimnazija.Application.BullyJournal;
using AutoMapper;
using MediatR;
using SkorinosGimnazija.Application.Common.Exceptions;
using SkorinosGimnazija.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BullyReports.Dtos;
using Dtos;
using Microsoft.EntityFrameworkCore;

public static class BullyJournalReportDetails
{
    public record Query(int Id) : IRequest<BullyJournalReportDetailsDto>;

    public class Handler : IRequestHandler<Query, BullyJournalReportDetailsDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BullyJournalReportDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.BullyJournalReports
                             .AsNoTracking()
                             .ProjectTo<BullyJournalReportDetailsDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}
