﻿namespace Application.Menus;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

public class MenuDetails
{
    public record Query(int Id) : IRequest<MenuDto?>;

    public class Handler : IRequestHandler<Query, MenuDto?>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MenuDto?> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Menus
                .AsNoTracking()
                .ProjectTo<MenuDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }
    }
}