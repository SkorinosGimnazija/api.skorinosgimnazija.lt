namespace SkorinosGimnazija.Application.IntegrationTests.Mocks;

using Common.Interfaces;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class CalendarServiceMock
{
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

    public Mock<ICalendarService> Mock { get; } = new();
}