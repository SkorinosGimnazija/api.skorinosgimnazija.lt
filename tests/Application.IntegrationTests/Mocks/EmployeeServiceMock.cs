namespace SkorinosGimnazija.Application.IntegrationTests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Moq;

using SkorinosGimnazija.Application.Common.Interfaces;
using SkorinosGimnazija.Application.IntegrationTests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;

public class EmployeeServiceMock
{
    public Mock<IEmployeeService> Mock { get; } = new();

    public EmployeeServiceMock(ServiceCollection services)
    {
        services.RemoveService<IEmployeeService>();
        services.AddTransient(_ => Mock.Object);

        Mock.Setup(x => x.GetEmployeeAsync(It.IsAny<string>()))
            .ReturnsAsync(
                new Employee
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "email",
                    FullName = "Name"
                });

        //Mock.Setup(x => x.GetPrincipalAsync());
        //Mock.Setup(x => x.GetHeadTeachersAsync());
        //Mock.Setup(x => x.GetTeachersAsync());

    }



}