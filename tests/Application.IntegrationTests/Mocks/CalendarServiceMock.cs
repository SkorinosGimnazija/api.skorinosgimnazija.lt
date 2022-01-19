namespace SkorinosGimnazija.Application.IntegrationTests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Moq;

using SkorinosGimnazija.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;

public class CalendarServiceMock
{
    public Mock<ICalendarService> Mock { get; } = new();

    public CalendarServiceMock(ServiceCollection services)
    {
        services.RemoveService<ICalendarService>();
        services.AddTransient(_ => Mock.Object);

        Mock.Setup(x =>
                x.AddAppointmentAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<string[]>()
                )
            )
            .ReturnsAsync(Guid.NewGuid().ToString());

        Mock.Setup(x =>
                x.AddEventAsync(
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<bool>()
                )
            )
            .ReturnsAsync(Guid.NewGuid().ToString());
    }
}
