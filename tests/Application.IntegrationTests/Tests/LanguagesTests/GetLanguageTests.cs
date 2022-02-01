namespace SkorinosGimnazija.Application.IntegrationTests.Tests.BannerTests;

using Domain.Entities;
using FluentAssertions;
using Languages;
using Xunit;

[Collection("App")]
public class GetLanguageTests
{
    private readonly AppFixture _app;

    public GetLanguageTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task PublicLanguageList_ShouldListLanguages()
    {
        var lang1 = await _app.AddAsync(new Language { Slug = Path.GetRandomFileName(), Name = "name1" });
        var lang2 = await _app.AddAsync(new Language { Slug = Path.GetRandomFileName(), Name = "name2" });

        var command = new PublicLanguageList.Query();

        var actual = await _app.SendAsync(command);

        actual.Select(x => x.Slug).Should().Contain(lang1.Slug, lang2.Slug);
    }
}