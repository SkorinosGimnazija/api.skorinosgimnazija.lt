namespace SkorinosGimnazija.Application.IntegrationTests.Tests.MetaTests;

using Domain.Entities.CMS;
using FluentAssertions;
using Meta;
using Xunit;

[Collection("App")]
public class GetMetaTests
{
    private readonly AppFixture _app;

    public GetMetaTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task MenuMetaList_ShouldListMenus_WithLinkedPosts()
    {
        var post = new Post
        {
            Title = "title",
            Slug = "slug",
            LanguageId = 1,
            IsPublished = true,
            PublishedAt = DateTime.UtcNow
        };

        await _app.AddAsync(post);

        var rootMenu = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            IsPublished = true,
            Slug = "slug1",
            Title = "title1",
            Path = "slug1"
        };

        await _app.AddAsync(rootMenu);

        var linkedMenu = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            LinkedPostId = post.Id,
            ParentMenuId = rootMenu.Id,
            IsPublished = true,
            Slug = "slug2",
            Title = "title2",
            Path = $"{rootMenu.Slug}/slug2"
        };

        var externalMenu = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            IsPublished = true,
            Url = "url",
            Slug = "slug3",
            Title = "title3",
            Path = "slug3"
        };

        await _app.AddAsync(linkedMenu);
        await _app.AddAsync(externalMenu);

        var command = new MenuMetaList.Query();

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(1);
        actual.Select(x => x.Url).Should().Contain($"/{rootMenu.Slug}/{linkedMenu.Slug}");
    }

    [Fact]
    public async Task PostMetaList_ShouldListPosts_WithoutMenuLinkedPosts()
    {
        var post = new Post
        {
            Title = "title1",
            Slug = "slug1",
            LanguageId = 1,
            IsPublished = true,
            PublishedAt = DateTime.UtcNow
        };

        await _app.AddAsync(post);

        var linkedPost = new Post
        {
            Title = "title",
            Slug = "slug",
            LanguageId = 1,
            IsPublished = true,
            PublishedAt = DateTime.UtcNow
        };

        await _app.AddAsync(linkedPost);

        var linkedMenu = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            LinkedPostId = linkedPost.Id,
            IsPublished = true,
            Slug = "slug2",
            Title = "title2",
            Path = "slug2"
        };

        await _app.AddAsync(linkedMenu);

        var command = new PostMetaList.Query();

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(1);
        actual.Select(x => x.Url).Should().Contain($"/{post.Id}/{post.Slug}");
    }
}