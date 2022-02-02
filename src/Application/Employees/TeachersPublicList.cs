namespace SkorinosGimnazija.Application.Employees;

using AutoMapper;
using Common.Interfaces;
using Dtos;
using MediatR;

public static class TeachersPublicList
{
    public record Query() : IRequest<List<EmployeeDto>>;

    public class Handler : IRequestHandler<Query, List<EmployeeDto>>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

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