namespace SkorinosGimnazija.Application.Users;
using MediatR;
using SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Application.Events.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Identity;

public static class TeachersPublicList
{
    public record Query() : IRequest<List<TeacherDto>>;

    public class Handler : IRequestHandler<Query, List<TeacherDto>>
    {
        private readonly ITeachersService _teachersService;

        public Handler(ITeachersService teachersService)
        {
            _teachersService = teachersService;
        }

        public async Task<List<TeacherDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return (await _teachersService.GetTeachersAsync(cancellationToken)).ToList();
        }
    }
}
