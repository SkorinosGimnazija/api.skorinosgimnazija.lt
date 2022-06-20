namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AccomplishmentsTests;

using Common.Exceptions;
using Accomplishments;
using Domain.Entities.Accomplishments;
using FluentAssertions;
using Menus;
using Xunit;
using SkorinosGimnazija.Infrastructure.Persistence.Migrations;

[Collection("App")]
public class GetAccomplishmentsTests
{
    private readonly AppFixture _app;
    private readonly int _currentUserId;

    public GetAccomplishmentsTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();

        var user = _app.CreateUserAsync().GetAwaiter().GetResult();
        _currentUserId = user.Id;

        _app.CurrentUserMock.SetCurrentUserData(_currentUserId, user.UserName);
    }

    [Fact]
    public async Task AccomplishmentList_ShouldThrowEx_WhenInvalidPagination()
    {
        var command = new AccomplishmentList.Query(new() { Page = int.MaxValue, Items = int.MaxValue });

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AccomplishmentAdminList_ShouldListAccomplishmentsByDate()
    {
        var accomplishment1 = await _app.AddAsync(new Accomplishment
        {
            Name = "Name1",
            Achievement = "Achievement1",
            Date = DateOnly.Parse("2021-02-01"),
            ScaleId = 1,
            UserId = _currentUserId
        });

        var accomplishment2 = await _app.AddAsync(new Accomplishment
        {
            Name = "Name2",
            Achievement = "Achievement2",
            Date = DateOnly.Parse("2021-03-01"),
            ScaleId = 1,
            UserId = _currentUserId
        });
 
        await _app.AddAsync(new Accomplishment
        {
            Name = "Name3",
            Achievement = "Achievement3",
            Date = DateOnly.Parse("2020-04-01"),
            ScaleId = 1,
            UserId = _currentUserId
        });

        var starDate = DateTime.Parse("2021-01-01");
        var endDate = DateTime.Parse("2021-12-31");

        var command = new AccomplishmentAdminList.Query(starDate, endDate);

        var actual = await _app.SendAsync(command);

        actual.Count.Should().Be(2);
        actual.Select(x => x.Id).Should().Contain(new[] { accomplishment1.Id, accomplishment2.Id });
        actual.Select(x => x.Date).Should().BeInDescendingOrder();
    }

    [Fact]
    public async Task AccomplishmentList_ShouldListOwnerAccomplishments()
    {
        var randomUser = await _app.CreateUserAsync();

        var accomplishment1 = await _app.AddAsync(new Accomplishment
        {
            Name = "Name1",
            Achievement = "Achievement1",
            Date = DateOnly.Parse("2021-02-01"),
            ScaleId = 1,
            UserId = _currentUserId
        });

        var accomplishment2 = await _app.AddAsync(new Accomplishment
        {
            Name = "Name2",
            Achievement = "Achievement2",
            Date = DateOnly.Parse("2021-03-01"),
            ScaleId = 1,
            UserId = _currentUserId
        });

        await _app.AddAsync(new Accomplishment
        {
            Name = "Name3",
            Achievement = "Achievement3",
            Date = DateOnly.Parse("2020-04-01"),
            ScaleId = 1,
            UserId = randomUser.Id
        });

        var command = new AccomplishmentList.Query(new());

        var actual = await _app.SendAsync(command);

        actual.TotalCount.Should().Be(2);
        actual.Items.Select(x => x.Id).Should().Contain(new[] { accomplishment1.Id, accomplishment2.Id });
    }

    [Fact]
    public async Task AccomplishmentDetails_ShouldGetOwnedAccomplishment()
    {
        var accomplishment = await _app.AddAsync(new Accomplishment
        {
            Name = "Name3",
            Achievement = "Achievement3",
            Date = DateOnly.Parse("2020-04-01"),
            ScaleId = 1,
            UserId = _currentUserId
        });

        var command = new AccomplishmentDetails.Query(accomplishment.Id);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(accomplishment.Id);
        actual.Name.Should().Be(accomplishment.Name);
        actual.Achievement.Should().Be(accomplishment.Achievement);
        actual.UserId.Should().Be(_currentUserId);
    }

    [Fact]
    public async Task AccomplishmentDetails_ShouldThrowEx_WhenAccessingInvalidAccomplishment()
    {
        var command = new AccomplishmentDetails.Query(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AccomplishmentDetails_ShouldThrowEx_WhenAccessingNotOwnedAccomplishment()
    {
        var owner = await _app.CreateUserAsync();

        var accomplishment = await _app.AddAsync(new Accomplishment
        {
            Name = "Name3",
            Achievement = "Achievement3",
            Date = DateOnly.Parse("2020-04-01"),
            ScaleId = 1,
            UserId = owner.Id
        });

        var command = new AccomplishmentDetails.Query(accomplishment.Id);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    
    [Fact]
    public async Task AccomplishmentScalesList_ShouldListAccomplishmentScales()
    {
        var command = new AccomplishmentScalesList.Query();

        var actual = await _app.SendAsync(command);

        actual.Count.Should().BeGreaterThan(0);
    }
}