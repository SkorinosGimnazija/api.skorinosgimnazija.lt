namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AppointmentTests;
using FluentAssertions;
using SkorinosGimnazija.Application.Appointments;
using SkorinosGimnazija.Application.Common.Exceptions;

using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Appointments;
using ParentAppointments;
using Xunit;

[Collection("App")]
public class UpdateAppointmentTests
{
    private readonly AppFixture _app;

    public UpdateAppointmentTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task AppointmentDelete_ShouldThrowNotFound()
    {
        var command = new AppointmentDelete.Command(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AppointmentDelete_ShouldDeleteAppointment()
    {
        var date = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.Now.AddDays(1),
            Type = new()
            {
                Id = 1,
                Name = "Name",
                Slug = "slug",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(7),
                RegistrationEnd = DateTime.Now,
                DurationInMinutes = 30,
                IsPublic = true,
                InvitePrincipal = true
            }
        });

        var entity = new Appointment
        {
            UserName = "username",
            AttendeeEmail = "b@gmail.com",
            AttendeeName = "Name1",
            EventId = Path.GetRandomFileName(),
            DateId = date.Id,
        };

        await _app.AddAsync(entity);

        var command = new AppointmentDelete.Command(entity.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Appointment>(entity.Id);

        actual.Should().BeNull();
    }


}
