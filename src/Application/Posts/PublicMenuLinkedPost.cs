namespace SkorinosGimnazija.Application.Menus;
using AutoMapper;
using MediatR;
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
using SkorinosGimnazija.Application.Posts.Dtos;
 
public  static class PublicMenuLinkedPost
{ 
    public record Query(string Path) : IRequest<PostDetailsDto>; 

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
                                     x.Path == path,
                                 cancellationToken);
            
            if (entity?.LinkedPost is null)
            {
                throw new NotFoundException();
            }

            return entity.LinkedPost;
        }
    }
}
