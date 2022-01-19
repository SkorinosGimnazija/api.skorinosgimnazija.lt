namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AppointmentTests;
using SkorinosGimnazija.Application.ParentAppointments;

using SkorinosGimnazija.Domain.Entities.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Exceptions;
using FluentAssertions;
using Xunit;
using SkorinosGimnazija.Application.Appointments;

[Collection("App")]
public class GetAppointmentDateTests
{
    private readonly AppFixture _app;
    private readonly string _currentUserName;

    public GetAppointmentDateTests(AppFixture app)
    {
        _app = app;
        _app.ResetData();

        var user = _app.CreateUserAsync().GetAwaiter().GetResult();
        _currentUserName = user.UserName;
        _app.CurrentUserMock.SetCurrentUserData(user.Id, _currentUserName);
    }

    [Fact]
    public async Task AppointmentDatesList_ShouldListDates_ByAppointmentTypeSlug()
    {
        var type1 = await _app.AddAsync(new AppointmentType
        {
            Name = "Name",
            Slug = "slug",
            Start = DateTime.Now,
            End = DateTime.Now.AddDays(7),
            RegistrationEnd = DateTime.Now,
            DurationInMinutes = 30,
            IsPublic = true,
            InvitePrincipal = true
        });

        var type2 = await _app.AddAsync(new AppointmentType
        {
            Name = "Name1",
            Slug = "slug1",
            Start = DateTime.Now,
            End = DateTime.Now.AddDays(7),
            RegistrationEnd = DateTime.Now,
            DurationInMinutes = 30,
            IsPublic = true,
            InvitePrincipal = true
        });

        var date1 = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.Now.AddDays(1),
            TypeId = type1.Id
        });
         
        var date2 = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.Now.AddDays(1),
            TypeId = type1.Id
        });

        await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.Now.AddDays(1),
            TypeId = type2.Id
        });

        var command = new AppointmentDatesList.Query(type1.Slug);

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(2);
        actual.Select(x => x.Id).Should().Contain(new[] { date1.Id, date2.Id });
    }

    [Fact]
    public async Task AppointmentAvailableDatesList_ShouldListAvailableDates_ByAppointmentTypeSlugAndUsername()
    {
        var type = await _app.AddAsync(new AppointmentType
        {
            Name = "Name",
            Slug = "slug",
            RegistrationEnd = DateTime.Now.AddDays(1),
            Start = DateTime.Now.AddDays(2),
            End = DateTime.Now.AddDays(7),
            DurationInMinutes = 30,
            IsPublic = true,
            InvitePrincipal = true
        });

        await _app.AddAsync(new AppointmentReservedDate
        {
            UserName = _currentUserName,
            Date = new ()
            {
                Date = DateTime.Now.AddDays(3),
                TypeId = type.Id
            }
        });

        await _app.AddAsync(new Appointment
        {
            UserName = _currentUserName,
            AttendeeEmail = "email",
            AttendeeName = "name",
            Date = new ()
            {
                Date = DateTime.Now.AddDays(4),
                TypeId = type.Id
            }
        });

        await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.Now.AddDays(-5),
            TypeId = type.Id
        });

        await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.Now.AddDays(15),
            TypeId = type.Id
        });

        var freeDate1 = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.Now.AddDays(6),
            TypeId = type.Id
        });

        var freeDate2 = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.Now.AddDays(5),
            TypeId = type.Id
        });

        var command = new AppointmentAvailableDatesList.Query(type.Slug, _currentUserName, false);

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(2);
        actual.Select(x => x.Id).Should().Contain(new[] { freeDate1.Id, freeDate2.Id });
        actual.Select(x => x.Date).Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task AppointmentAvailableDatesList_ShouldThrowEx_WhenPublicRequestWithPrivateType()
    {
        var type = await _app.AddAsync(new AppointmentType
        {
            Name = "Name",
            Slug = "slug",
            RegistrationEnd = DateTime.Now.AddDays(1),
            Start = DateTime.Now.AddDays(2),
            End = DateTime.Now.AddDays(7),
            DurationInMinutes = 30,
            IsPublic = false,
            InvitePrincipal = true
        });

        var command = new AppointmentAvailableDatesList.Query(type.Slug, _currentUserName, true);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }


    [Fact]
    public async Task AppointmentAvailableDatesList_ShouldThrowEx_WhenInvalidType()
    {
        var command = new AppointmentAvailableDatesList.Query("type", _currentUserName, false);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}
