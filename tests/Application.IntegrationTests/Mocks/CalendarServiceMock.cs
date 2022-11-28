namespace SkorinosGimnazija.Application.IntegrationTests.Mocks;

using Common.Interfaces;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SkorinosGimnazija.Application.Common.Models;

public class CalendarServiceMock
{
    public CalendarServiceMock(ServiceCollection services)
    {
        services.RemoveService<ICalendarService>();
        services.AddTransient(_ => Mock.Object);

        Mock.Setup(x => x.AddAppointmentAsync(It.IsAny<AppointmentEvent>()))
            .ReturnsAsync(new AppointmentEventResponse
                { EventId = Guid.NewGuid().ToString(), EventMeetingLink = Guid.NewGuid().ToString() });

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