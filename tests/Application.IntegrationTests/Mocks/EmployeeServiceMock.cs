namespace SkorinosGimnazija.Application.IntegrationTests.Mocks;

using Common.Interfaces;
using Domain.Entities.Identity;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class EmployeeServiceMock
{
    public EmployeeServiceMock(ServiceCollection services)
    {
        services.RemoveService<IEmployeeService>();
        services.AddTransient(_ => Mock.Object);

        Mock.Setup(x => x.GetPrincipalAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Employee
            {
                Id = Guid.NewGuid().ToString(),
                Email = "principal@email",
                FullName = "Principal Name"
            });
    }

    public Mock<IEmployeeService> Mock { get; } = new();

    public void SetEmployeeData(string userName, string email)
    {
        Mock.Setup(x => x.GetEmployeeAsync(It.Is<string>(z => z == userName)))
            .ReturnsAsync(
                new Employee
                {
                    Id = userName,
                    Email = email,
                    FullName = "Employee Name"
                });
    }
}