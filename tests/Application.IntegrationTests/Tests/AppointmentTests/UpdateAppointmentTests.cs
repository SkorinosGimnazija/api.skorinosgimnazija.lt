namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AppointmentTests;

using Appointments;
using Common.Exceptions;
using Domain.Entities.Appointments;
using FluentAssertions;
using Xunit;

[Collection("App")]
public class UpdateAppointmentTests
{
    private readonly AppFixture _app;
    private readonly string _currentUserName;

    public UpdateAppointmentTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();

        var user = _app.CreateUserAsync().GetAwaiter().GetResult();
        _currentUserName = user.UserName!;

        _app.CurrentUserMock.SetCurrentUserData(user.Id, user.UserName!);
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
            UserName = "username",
            AttendeeEmail = "b@gmail.com",
            AttendeeName = "Name1",
            UserDisplayName = "User Name 1",
            AttendeeUserName = _currentUserName,
            EventId = Path.GetRandomFileName(),
            DateId = date.Id
        };

        await _app.AddAsync(entity);

        var command = new AppointmentDelete.Command(entity.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Appointment>(entity.Id);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task AppointmentDelete_ShouldNotDeleteAppointment_WhenNotAttendee()
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
            UserName = "username",
            AttendeeEmail = "b@gmail.com",
            AttendeeName = "Name1",
            UserDisplayName = "User Name",
            AttendeeUserName = "random_user_name",
            EventId = Path.GetRandomFileName(),
            DateId = date.Id
        };

        await _app.AddAsync(entity);

        var command = new AppointmentDelete.Command(entity.Id);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }
}