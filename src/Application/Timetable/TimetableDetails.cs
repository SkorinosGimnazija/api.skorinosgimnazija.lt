namespace SkorinosGimnazija.Application.Timetable;
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
using Dtos;
using Microsoft.EntityFrameworkCore;

public static class TimetableDetails
{
    public record Query(int Id) : IRequest<TimetableDto>;

    public class Handler : IRequestHandler<Query, TimetableDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TimetableDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.Timetable
                             .AsNoTracking()
                             .ProjectTo<TimetableDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}
