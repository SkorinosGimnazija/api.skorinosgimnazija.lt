namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AppointmentTests;

using Appointments;
using Common.Exceptions;
using Domain.Entities.Appointments;
using FluentAssertions;
using Xunit;

[Collection("App")]
public class GetAppointmentTests
{
    private readonly AppFixture _app;
    private readonly string _currentUserName;

    public GetAppointmentTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();

        var user = _app.CreateUserAsync().GetAwaiter().GetResult();
        _currentUserName = user.UserName;
        _app.CurrentUserMock.SetCurrentUserData(user.Id, _currentUserName);
    }

    [Fact]
    public async Task AppointmentAdminList_ShouldThrowEx_WhenInvalidPagination()
    {
        var command = new AppointmentAdminList.Query(new() { Items = int.MaxValue, Page = int.MaxValue });

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AppointmentAdminList_ShouldPaginateAppointments()
    {
        var date = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.UtcNow.AddDays(1),
            Type = new()
            {
                Id = 1,
                Name = "Name",
                Slug = "slug",
                RegistrationEnd = DateTime.UtcNow,
                DurationInMinutes = 30,
                IsPublic = true,
                InvitePrincipal = true
            }
        });

        var entity1 = new Appointment
        {
            UserName = "username",
            AttendeeEmail = "a@gmail.com",
            AttendeeName = "Name",
            EventId = Path.GetRandomFileName(),
            DateId = date.Id
        };

        var entity2 = new Appointment
        {
            UserName = "username1",
            AttendeeEmail = "b@gmail.com",
            AttendeeName = "Name1",
            EventId = Path.GetRandomFileName(),
            DateId = date.Id
        };

        await _app.AddAsync(entity1);
        await _app.AddAsync(entity2);

        var command = new AppointmentAdminList.Query(new());

        var actual = await _app.SendAsync(command);

        actual.Items.Should().HaveCount(2);
        actual.Items.Select(x => x.Id).Should().Contain(new[] { entity1.Id, entity2.Id });
        actual.TotalCount.Should().Be(2);
        actual.PageNumber.Should().Be(0);
        actual.TotalPages.Should().Be(1);
    }

    [Fact]
    public async Task AppointmentList_ShouldThrowEx_WhenInvalidPagination()
    {
        var command = new AppointmentToUserList.Query("any", new() { Items = int.MaxValue, Page = int.MaxValue });

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AppointmentList_ShouldListOwnerAppointments()
    {
        var randomUser = await _app.CreateUserAsync();

        var date = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.UtcNow.AddDays(1),
            Type = new()
            {
                Id = 1,
                Name = "Name",
                Slug = "slug",
                RegistrationEnd = DateTime.UtcNow,
                DurationInMinutes = 30,
                IsPublic = true,
                InvitePrincipal = true
            }
        });

        var entity1 = new Appointment
        {
            UserName = randomUser.UserName,
            AttendeeEmail = "a@gmail.com",
            AttendeeName = "Name",
            EventId = Path.GetRandomFileName(),
            DateId = date.Id
        };

        var entity2 = new Appointment
        {
            UserName = _currentUserName,
            AttendeeEmail = "b@gmail.com",
            AttendeeName = "Name1",
            EventId = Path.GetRandomFileName(),
            DateId = date.Id
        };

        await _app.AddAsync(entity1);
        await _app.AddAsync(entity2);

        var command = new AppointmentToUserList.Query(date.Type.Slug, new());

        var actual = await _app.SendAsync(command);

        actual.Items.Should().HaveCount(1);
        actual.Items.Select(x => x.Id).Should().Contain(new[] { entity2.Id });
        actual.TotalCount.Should().Be(1);
        actual.PageNumber.Should().Be(0);
        actual.TotalPages.Should().Be(1);
    }

    [Fact]
    public async Task AppointmentDetails_ShouldThrowEx_WhenInvalidId()
    {
        var command = new AppointmentDetails.Query(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AppointmentDetails_ShouldGetAppointment_ById()
    {
        var date = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.UtcNow.AddDays(1),
            Type = new()
            {
                Id = 1,
                Name = "Name",
                Slug = "slug",
                RegistrationEnd = DateTime.UtcNow,
                DurationInMinutes = 30,
                IsPublic = true,
                InvitePrincipal = true
            }
        });

        var entity = new Appointment
        {
            UserName = _currentUserName,
            AttendeeEmail = "b@gmail.com",
            AttendeeName = "Name1",
            EventId = Path.GetRandomFileName(),
            DateId = date.Id
        };

        await _app.AddAsync(entity);

        var command = new AppointmentDetails.Query(entity.Id);

        var actual = await _app.SendAsync(command);

        actual.AttendeeEmail.Should().Be(entity.AttendeeEmail);
        actual.AttendeeName.Should().Be(entity.AttendeeName);
        actual.EventId.Should().Be(entity.EventId);
        actual.DateId.Should().Be(entity.DateId);
        actual.UserName.Should().Be(entity.UserName);
    }
}