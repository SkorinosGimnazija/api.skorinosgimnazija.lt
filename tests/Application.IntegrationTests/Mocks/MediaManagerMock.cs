namespace SkorinosGimnazija.Application.IntegrationTests.Mocks;

using Common.Interfaces;
using Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class MediaManagerMock
{
    private readonly Mock<IMediaManager> _mock = new();
    private readonly List<string> _mockData = new();

    public MediaManagerMock(ServiceCollection services)
    {
        _mock.Setup(x => x.SaveFilesAsync(It.IsAny<IEnumerable<IFormFile>>()))
            .ReturnsAsync(_mockData);

        services.RemoveService<IMediaManager>();
        services.AddTransient(_ => _mock.Object);
    }

    public void SetReturnFilesData(IEnumerable<string> files)
    {
        _mockData.Clear();
        _mockData.AddRange(files);
    }
}