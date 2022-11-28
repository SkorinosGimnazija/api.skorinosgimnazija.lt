namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AppointmentTests;

using Appointments;
using Common.Exceptions;
using Domain.Entities.Appointments;
using FluentAssertions;
using Xunit;

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
    public async Task AppointmentDatesList_ShouldListDates_ByAppointmentTypeId()
    {
        var type1 = await _app.AddAsync(new AppointmentType
        {
            Name = "Name",
            Slug = "slug",
            RegistrationEnd = DateTime.UtcNow,
            DurationInMinutes = 30,
            IsPublic = true,
            InvitePrincipal = true
        });

        var type2 = await _app.AddAsync(new AppointmentType
        {
            Name = "Name1",
            Slug = "slug1",
            RegistrationEnd = DateTime.UtcNow,
            DurationInMinutes = 30,
            IsPublic = true,
            InvitePrincipal = true
        });

        var date1 = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.UtcNow.AddDays(1),
            TypeId = type1.Id
        });

        var date2 = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.UtcNow.AddDays(1),
            TypeId = type1.Id
        });

        await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.UtcNow.AddDays(1),
            TypeId = type2.Id
        });

        var command = new AppointmentDatesList.Query(type1.Id);

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
            RegistrationEnd = DateTime.UtcNow.AddDays(1),
            DurationInMinutes = 30,
            IsPublic = true,
            InvitePrincipal = true
        });

        await _app.AddAsync(new AppointmentReservedDate
        {
            UserName = _currentUserName,
            Date = new()
            {
                Date = DateTime.UtcNow.AddDays(3),
                TypeId = type.Id
            }
        });

        await _app.AddAsync(new Appointment
        {
            UserName = _currentUserName,
            AttendeeEmail = "email",
            AttendeeName = "name",
            EventId = "eventId",
            UserDisplayName= "display name",
            Date = new()
            {
                Date = DateTime.UtcNow.AddDays(4),
                TypeId = type.Id
            }
        });

        await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.UtcNow.AddDays(-5),
            TypeId = type.Id
        });

        var freeDate1 = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.UtcNow.AddDays(15),
            TypeId = type.Id
        });

        var freeDate2 = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.UtcNow.AddDays(6),
            TypeId = type.Id
        });

        var freeDate3 = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.UtcNow.AddDays(5),
            TypeId = type.Id
        });

        var command = new AppointmentAvailableDatesList.Query(type.Slug, _currentUserName, true);

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(3);
        actual.Select(x => x.Id).Should().Contain(new[] { freeDate1.Id, freeDate2.Id, freeDate3.Id });
        actual.Select(x => x.Date).Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task AppointmentAvailableDatesList_ShouldThrowEx_WhenPublicRequestWithPrivateType()
    {
        var type = await _app.AddAsync(new AppointmentType
        {
            Name = "Name",
            Slug = "slug",
            RegistrationEnd = DateTime.UtcNow.AddDays(1),
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