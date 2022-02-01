namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AppointmentTypeTests;

using Appointments;
using Appointments.Dtos;
using Common.Exceptions;
using Domain.Entities.Appointments;
using FluentAssertions;
using ParentAppointments;
using Xunit;

[Collection("App")]
public class UpdateAppointmentTypeTypeTests
{
    private readonly AppFixture _app;

    public UpdateAppointmentTypeTypeTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task AppointmentTypeDelete_ShouldThrowNotFound()
    {
        var command = new AppointmentTypeDelete.Command(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AppointmentTypeDelete_ShouldDeleteAppointmentType()
    {
        var entity = new AppointmentType
        {
            Name = "Name",
            Slug = "slug",
            Start = DateTime.Now,
            End = DateTime.Now.AddDays(7),
            RegistrationEnd = DateTime.Now,
            DurationInMinutes = 30,
            IsPublic = true,
            InvitePrincipal = true
        };

        await _app.AddAsync(entity);

        var command = new AppointmentTypeDelete.Command(entity.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<AppointmentType>(entity.Id);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task AppointmentTypeEdit_ShouldThrowValidationException()
    {
        var entityDto = new AppointmentTypeEditDto();
        var command = new AppointmentTypeEdit.Command(entityDto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AppointmentTypeEdit_ShouldThrowNotFoundException()
    {
        var entity = new AppointmentTypeEditDto
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
        };

        var command = new AppointmentTypeEdit.Command(entity);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AppointmentTypeEdit_ShouldEditAppointmentType()
    {
        var entity = new AppointmentType
        {
            Name = "Name",
            Slug = "slug",
            Start = DateTime.Now,
            End = DateTime.Now.AddDays(7),
            RegistrationEnd = DateTime.Now,
            DurationInMinutes = 30,
            IsPublic = true,
            InvitePrincipal = true
        };

        await _app.AddAsync(entity);

        var entityDto = new AppointmentTypeEditDto
        {
            Id = entity.Id,
            Name = "Name1",
            Slug = "slug1",
            Start = DateTime.Now.AddDays(1),
            End = DateTime.Now.AddDays(8),
            RegistrationEnd = DateTime.Now.AddDays(1),
            DurationInMinutes = 45,
            IsPublic = false,
            InvitePrincipal = false
        };

        var command = new AppointmentTypeEdit.Command(entityDto);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<AppointmentType>(entityDto.Id);

        actual.Should().NotBeNull();
        actual.Name.Should().Be(entityDto.Name);
        actual.Slug.Should().Be(entityDto.Slug);
        actual.DurationInMinutes.Should().Be(entityDto.DurationInMinutes);
        actual.IsPublic.Should().Be(entityDto.IsPublic);
        actual.InvitePrincipal.Should().Be(entityDto.InvitePrincipal);
        actual.Start.Should().BeCloseTo(entityDto.Start, TimeSpan.FromSeconds(5));
        actual.End.Should().BeCloseTo(entityDto.End, TimeSpan.FromSeconds(5));
        actual.RegistrationEnd.Should().BeCloseTo(entityDto.RegistrationEnd, TimeSpan.FromSeconds(5));
    }
}