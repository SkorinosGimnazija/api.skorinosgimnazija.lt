﻿namespace SkorinosGimnazija.Application.Appointments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AppointmentDatesList
{
    public record Query(int TypeId) : IRequest<List<AppointmentDateDto>>;

    public class Handler : IRequestHandler<Query, List<AppointmentDateDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AppointmentDateDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDates.AsNoTracking()
                       .Where(x => x.TypeId == request.TypeId)
                       .OrderBy(x => x.Date)
                       .ProjectTo<AppointmentDateDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}