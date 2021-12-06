namespace SkorinosGimnazija.Application.Languages;
using AutoMapper;
using MediatR;
using SkorinosGimnazija.Application.Languages.Dtos;

using SkorinosGimnazija.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public static class PublicLanguageList 
{
    public record Query() : IRequest<List<LanguageDto>>;

    public class Handler : IRequestHandler<Query, List<LanguageDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
           
        public async Task<List<LanguageDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Languages
                       .AsNoTracking()
                       .ProjectTo<LanguageDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}
