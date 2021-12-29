namespace SkorinosGimnazija.Infrastructure.UnitTests;

using Application.Common.Exceptions;
using Application.Common.Interfaces;
using FileManagement;
using FluentAssertions;
using Google.Apis.Util;
using ImageOptimization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Options;
using Xunit;

public class MediaManagerTests
{
    private readonly Mock<IFileService> _fileServiceMock = new();
    private readonly Mock<IImageOptimizer> _imageOptimizerMock = new();
    private readonly MediaManagerOptions _mediaManagerOptions = new() { UploadPath = new[] { "random-path" } };
    private readonly MediaManager _sut;
    private readonly UrlOptions _urlOptions = new() { Domain = "domain", Static = "static.local" };

    public MediaManagerTests()
    {
        var fileManagerOptions = new Mock<IOptions<MediaManagerOptions>>();
        fileManagerOptions.Setup(x => x.Value).Returns(_mediaManagerOptions);

        var urlOptions = new Mock<IOptions<UrlOptions>>();
        urlOptions.Setup(x => x.Value).Returns(_urlOptions);

        _sut = new(
            fileManagerOptions.Object,
            urlOptions.Object,
            _imageOptimizerMock.Object,
            _fileServiceMock.Object);
    }

    [Fact]
    public void GenerateFileLinks_ShouldReplaceTemplateWithLinksInText()
    {
        var staticUrl = _urlOptions.Static;
        var text = $"text1 text [link]({MediaManager.FileUrlReplaceTemplate}/file.pdf) text text text" +
                   $"text text [link]({MediaManager.FileUrlReplaceTemplate}/file 1.pdf) \n\n\n" +
                   $"asd asd [link]({MediaManager.FileUrlReplaceTemplate}/file@55.docx)asda";
        var files = new List<string>
        {
            "random-path/file.pdf",
            "random-path/file 1.pdf",
            "random-path2/file@55.docx"
        };
          
        var actual = _sut.GenerateFileLinks(text, files);

        actual.Should().Be($"text1 text [link]({staticUrl}/random-path/file.pdf) text text text" +
                           $"text text [link]({staticUrl}/random-path/file%201.pdf) \n\n\n" +
                           $"asd asd [link]({staticUrl}/random-path2/file%4055.docx)asda");
    }

    [Fact]
    public void GenerateFileLinks_ShouldReturnText_WhenFilesAreNull()
    {
        var text = "text text text";

        var actual = _sut.GenerateFileLinks(text, null);

        actual.Should().Be(text);
    }

    [Fact]
    public void GenerateFileLinks_ShouldReturnNull_WhenTextIsNull()
    {
        var files = new List<string>
        {
            "random-path/file.pdf",
            "random-path/file 1.pdf",
            "random-path2/file@55.docx"
        };

        var actual = _sut.GenerateFileLinks(null, files);

        actual.Should().BeNull();
    }

     
    [Fact]
    public void GenerateFileLinks_ShouldReturnText_WhenFileNotFound()
    {
        var text = $"text1 text [link]({MediaManager.FileUrlReplaceTemplate}/file.pdf) text text text";
        var files = new List<string>();

         var actual = _sut.GenerateFileLinks(text, files);

         actual.Should().Be(text);
    }

    [Fact]
    public async Task SaveFilesAsync_ShouldReturnSavedFilesWithRandomPath()
    {
        var files = new List<IFormFile> 
        {
           new FormFile(null!, 0, 0, null!, "FileName1.pdf"),
           new FormFile(null!, 0, 0, null!, "FileName2.doc"),
           new FormFile(null!, 0, 0, null!, "FileName3.docx"),
        }; 

        var actual =  await  _sut.SaveFilesAsync( files);

        actual.Should().HaveCount(files.Count);
        actual.Should().NotContain(files.Select(x => x.FileName));

        _fileServiceMock.
            Verify(x => x.SaveFileAsync(It.IsIn(files.AsEnumerable()), It.IsAny<string>()),
            Times.Exactly(files.Count));
    }

    [Fact]
    public async Task SaveImagesAsync_ShouldOptimizeAndReturnFilesWithRandomPath()
    {
        var files = new List<IFormFile>
        {
            new FormFile(null!, 0, 0, null!, "FileName1.jpg"),
            new FormFile(null!, 0, 0, null!, "FileName2.jpg"),
            new FormFile(null!, 0, 0, null!, "FileName3.png"),
        };

        _fileServiceMock
            .Setup(x => x.DownloadFileAsync(It.IsAny<Uri>(), It.IsAny<string>()))
            .ReturnsAsync(Path.GetRandomFileName);

        var actual = await _sut.SaveImagesAsync(files, true);

        actual.Should().HaveCount(files.Count);
        actual.Should().NotContain(files.Select(x => x.FileName));

        _fileServiceMock. 
            Verify(x => x.DownloadFileAsync(It.IsAny<Uri>(), It.IsAny<string>()),
                Times.Exactly(files.Count));

        _imageOptimizerMock.
            Verify(x => x.OptimizeAsync(It.IsIn(files.AsEnumerable()), It.IsAny<string>()),
                Times.Exactly(files.Count));
    }
}