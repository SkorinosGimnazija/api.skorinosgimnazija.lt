namespace SkorinosGimnazija.Application.Appointments;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Common.Exceptions;
using SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Application.ParentAppointments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
 
public static class AppointmentTypeDetails
{
    public record Query(int Id) : IRequest<AppointmentTypeDto>;

    public class Handler : IRequestHandler<Query, AppointmentTypeDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
         
        public async Task<AppointmentTypeDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.AppointmentTypes
                             .AsNoTracking()
                             .ProjectTo<AppointmentTypeDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}
