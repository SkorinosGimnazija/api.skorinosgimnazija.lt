namespace SkorinosGimnazija.Application.Users;
using MediatR;
using SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Application.Events.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.Identity;
using SkorinosGimnazija.Domain.Entities.Teacher;

public static class TeachersPublicList
{
    public record Query() : IRequest<List<EmployeeDto>>;

    public class Handler : IRequestHandler<Query, List<EmployeeDto>>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public Handler(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }
         
        public async Task<List<EmployeeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var teachers = await _employeeService.GetTeachersAsync(cancellationToken);
            return _mapper.Map<List<EmployeeDto>>(teachers);
        }
    }
}
