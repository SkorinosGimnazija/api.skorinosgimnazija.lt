namespace SkorinosGimnazija.Application.IntegrationTests.Tests.PostTests;

using Common.Exceptions;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Posts;
using SkorinosGimnazija.Application.Menus;
using Xunit;

[Collection("App")]
public class GetPostTests
{
    private readonly AppFixture _app;

    public GetPostTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetDatabase();
    }

    [Fact]
    public async Task PostDetails_ShouldThrowNotFoundException()
    {
        var command = new PostDetails.Query(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task PostDetails_ShouldFindPost()
    {
        var post = new Post
        {
            LanguageId = 1,
            Slug = "slug",
            Title = "title"
        };

        await _app.AddAsync(post);

        var command = new PostDetails.Query(post.Id);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
    } 

    [Fact]
    public async Task PublicPostList_ShouldThrowEx_WhenInvalidPagination()
    {
        var command = new PublicPostList.Query("language", new() { Items = int.MaxValue, Page = int.MaxValue });

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task PublicPostList_ShouldThrowValidationExWithBigItemsNumber()
    {
        var command = new PublicPostList.Query("language", new() { Items = int.MaxValue, Page = 0 });

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Theory]
    [InlineData(2, null, true, true)]
    [InlineData(1, null, false, true)]
    [InlineData(1, null, false, false)]
    [InlineData(1, null, true, false)]
    [InlineData(1, 1, true, true)]
    [InlineData(1, 1, false, true)]
    public async Task PublicPostList_ShouldListPublishedInFeedPostsByLanguageInOrder(
        int expected, int? langId, bool isPublished, bool showInFeed)
    {
        var lang = await _app.AddAsync(new Language { Slug = Path.GetRandomFileName(), Name = "name" });
        var rng = new Random(lang.Id);

        var post = new Post
        {
            Slug = "slug",
            Title = "title",
            LanguageId = langId ?? lang.Id,
            ShowInFeed = showInFeed,
            IsPublished = isPublished,
            PublishDate = DateTime.UtcNow.AddDays(rng.Next(-20, -10))
        };

        var postDummy = new Post
        {
            Slug = "slug",
            Title = "title",
            LanguageId = lang.Id,
            ShowInFeed = true,
            IsPublished = true,
            PublishDate = DateTime.UtcNow.AddDays(-15)
        };

        await _app.AddAsync(post);
        await _app.AddAsync(postDummy);

        var command = new PublicPostList.Query(lang.Slug, new());

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(expected);
        actual.Select(x => x.PublishDate).Should().BeInDescendingOrder();
    }

    [Fact]
    public async Task PublicPostList_ShouldOrderByFeaturedFirst()
    {
        var lang = await _app.AddAsync(new Language { Slug = Path.GetRandomFileName(), Name = "name" });

        var post1 = new Post
        {
            LanguageId = lang.Id,
            Slug = "slug",
            Title = "title",
            ShowInFeed = true,
            IsPublished = true,
            IsFeatured = true,
            PublishDate = DateTime.UtcNow.AddDays(-10)
        };

        var post2 = new Post
        {
            LanguageId = lang.Id,
            Slug = "slug",
            Title = "title",
            ShowInFeed = true,
            IsPublished = true,
            IsFeatured = false,
            PublishDate = DateTime.UtcNow
        };

        await _app.AddAsync(post1);
        await _app.AddAsync(post2);

        var command = new PublicPostList.Query(lang.Slug, new());

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(2);
        actual.Select(x => x.Id).Should().ContainInOrder(post1.Id, post2.Id);
    }

    [Fact]
    public async Task PublicPostList_ShouldNotListPublishedNotInFeedPosts()
    {
        var lang = await _app.AddAsync(new Language { Slug = Path.GetRandomFileName(), Name = "name" });

        var post = new Post
        {
            LanguageId = lang.Id,
            Slug = "slug",
            Title = "title",
            ShowInFeed = false,
            IsPublished = true,
            PublishDate = DateTime.MinValue
        };

        await _app.AddAsync(post);

        var command = new PublicPostList.Query(lang.Slug, new());

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(0);
    }

    [Fact]
    public async Task PublicPostDetails_ShouldNotGetUnpublishedPost()
    {
        var post = new Post
        {
            Id = 1,
            LanguageId = 1,
            Slug = "slug",
            Title = "title",
            IsPublished = false,
            PublishDate = DateTime.MinValue
        };

        await _app.AddAsync(post);

        var command = new PublicPostDetails.Query(post.Id);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }
     
    [Fact]
    public async Task PublicMenuLinkedPost_ShouldReturnPostByMenuPath()
    {
        var lang = await _app.AddAsync(new Language { Slug = Path.GetRandomFileName(), Name = "name" });

        var post = new Post
        {
            LanguageId = 1,
            Slug = "post-slug",
            Title = "post title",
            IsPublished = true,
            PublishDate = DateTime.UtcNow
        };

        await _app.AddAsync(post);

        var menu = new Menu
        {
            LanguageId = lang.Id,
            MenuLocationId = 1,
            Title = "menu title",
            Slug = "menu-slug",
            Path = "/menu-slug/title",
            IsPublished = true,
            LinkedPostId = post.Id
        };
         
        await _app.AddAsync(menu);

        var path = Uri.EscapeDataString(menu.Path);

        var command = new PublicMenuLinkedPost.Query(lang.Slug, path);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(post.Id);
        actual.Slug.Should().Be(post.Slug);
        actual.Title.Should().Be(post.Title);
}

[Fact]
    public async Task PublicPostDetails_ShouldNotGetPublishedInTheFuturePost()
    {
        var post = new Post
        {
            Id = 1,
            LanguageId = 1,
            Slug = "slug",
            Title = "title",
            IsPublished = true,
            PublishDate = DateTime.MaxValue
        };

        await _app.AddAsync(post);

        var command = new PublicPostDetails.Query(post.Id);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task PublicPostSearchList_ShouldSearchPublishedPostsInOrder()
    {
        var post1 = new Post
        {
            Id = 1,
            LanguageId = 1,
            Slug = "slug",
            Title = "title",
            IsPublished = true,
            PublishDate = DateTime.UtcNow
        };

        var post2 = new Post
        {
            Id = 2,
            LanguageId = 1,
            Slug = "slug",
            Title = "title",
            IsPublished = true,
            PublishDate = DateTime.UtcNow.AddDays(3)
        };

        var post3 = new Post
        {
            Id = 3,
            LanguageId = 1,
            Slug = "slug",
            Title = "title",
            IsPublished = false,
            PublishDate = DateTime.UtcNow
        };

        var post4 = new Post
        {
            Id = 4,
            LanguageId = 1,
            Slug = "slug",
            Title = "title",
            IsPublished = true,
            PublishDate = DateTime.UtcNow.AddDays(-1)
        };

        await _app.AddAsync(post1);
        await _app.AddAsync(post2);
        await _app.AddAsync(post3);
        await _app.AddAsync(post4);

        _app.SearchClientMock.SetReturnData(new[] { post4.Id, post2.Id, post1.Id, post3.Id });

        var command = new PublicPostSearchList.Query("ignored-search-query", new());

        var actual = await _app.SendAsync(command);

        actual.Items.Should().HaveCount(2);
        actual.TotalCount.Should().Be(4);
        actual.Items.Select(x => x.Id).Should().ContainInOrder(post4.Id, post1.Id);
    }
}