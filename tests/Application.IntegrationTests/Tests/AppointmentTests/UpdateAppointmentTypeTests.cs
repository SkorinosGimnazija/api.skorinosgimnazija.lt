namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AppointmentTests;

using Appointments;
using Appointments.Dtos;
using Common.Exceptions;
using Domain.Entities.Appointments;
using FluentAssertions;
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
        actual.RegistrationEnd.Should().BeCloseTo(entityDto.RegistrationEnd, TimeSpan.FromSeconds(5));
    }
}