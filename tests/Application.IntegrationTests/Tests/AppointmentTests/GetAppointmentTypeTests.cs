namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AppointmentTests;

using Appointments;
using Common.Exceptions;
using Domain.Entities.Appointments;
using FluentAssertions;
using ParentAppointments;
using Xunit;

[Collection("App")]
public class GetAppointmentTypeTests
{
    private readonly AppFixture _app;

    public GetAppointmentTypeTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task AppointmentTypeDetails_ShouldThrowNotFoundException()
    {
        var command = new AppointmentTypeDetails.Query(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AppointmentTypeDetails_ShouldFindById()
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

        var command = new AppointmentTypeDetails.Query(entity.Id);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
        actual.Name.Should().Be(entity.Name);
        actual.Slug.Should().Be(entity.Slug);
        actual.DurationInMinutes.Should().Be(entity.DurationInMinutes);
        actual.IsPublic.Should().Be(entity.IsPublic);
        actual.InvitePrincipal.Should().Be(entity.InvitePrincipal);
        actual.Start.Should().BeCloseTo(entity.Start, TimeSpan.FromSeconds(5));
        actual.End.Should().BeCloseTo(entity.End, TimeSpan.FromSeconds(5));
        actual.RegistrationEnd.Should().BeCloseTo(entity.RegistrationEnd, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task AppointmentTypePublicDetails_ShouldThrowNotFoundException()
    {
        var command = new AppointmentTypePublicDetails.Query("slug");

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AppointmentTypePublicDetails_ShouldFindBySlug()
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

        var command = new AppointmentTypePublicDetails.Query(entity.Slug);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(entity.Id);
        actual.Name.Should().Be(entity.Name);
        actual.DurationInMinutes.Should().Be(entity.DurationInMinutes);
        actual.IsPublic.Should().Be(entity.IsPublic);
        actual.InvitePrincipal.Should().Be(entity.InvitePrincipal);
        actual.Start.Should().BeCloseTo(entity.Start, TimeSpan.FromSeconds(5));
        actual.End.Should().BeCloseTo(entity.End, TimeSpan.FromSeconds(5));
        actual.RegistrationEnd.Should().BeCloseTo(entity.RegistrationEnd, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task AppointmentTypesList_ShouldListAppointmentTypes()
    {
        var entity1 = new AppointmentType
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

        var entity2 = new AppointmentType
        {
            Name = "Name1",
            Slug = "slug1",
            Start = DateTime.Now,
            End = DateTime.Now.AddDays(7),
            RegistrationEnd = DateTime.Now,
            DurationInMinutes = 30,
            IsPublic = true,
            InvitePrincipal = true
        };

        await _app.AddAsync(entity1);
        await _app.AddAsync(entity2);

        var command = new AppointmentTypesList.Query();

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(2);
        actual.Select(x => x.Id).Should().Contain(new[] { entity1.Id, entity2.Id });
    }

    [Fact]
    public async Task AppointmentTypeDetails_ShouldThrowEx_WhenInvalidId()
    {
        var command = new AppointmentTypeDetails.Query(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AppointmentTypeDetails_ShouldGetAppointmentType_ById()
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

        var command = new AppointmentTypeDetails.Query(entity.Id);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(entity.Id);
        actual.Name.Should().Be(entity.Name);
        actual.DurationInMinutes.Should().Be(entity.DurationInMinutes);
        actual.IsPublic.Should().Be(entity.IsPublic);
        actual.InvitePrincipal.Should().Be(entity.InvitePrincipal);
        actual.Start.Should().BeCloseTo(entity.Start, TimeSpan.FromSeconds(5));
        actual.End.Should().BeCloseTo(entity.End, TimeSpan.FromSeconds(5));
        actual.RegistrationEnd.Should().BeCloseTo(entity.RegistrationEnd, TimeSpan.FromSeconds(5));
    }
}