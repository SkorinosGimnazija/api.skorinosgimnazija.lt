namespace SkorinosGimnazija.Application.ParentAppointments;
using AutoMapper;
using MediatR;
using SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Application.Menus.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using MenuLocations;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.ParentAppointments.Dtos;

public  static class AppointmentTypesList
{
    public record Query() : IRequest<List<AppointmentTypeDto>>;

    public class Handler : IRequestHandler<Query, List<AppointmentTypeDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        } 

        public async Task<List<AppointmentTypeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.AppointmentTypes
                       .AsNoTracking()
                       .ProjectTo<AppointmentTypeDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}