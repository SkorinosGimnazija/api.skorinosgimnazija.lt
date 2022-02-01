namespace SkorinosGimnazija.Application.IntegrationTests.Mocks;

using Common.Interfaces;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class CaptchaServiceMock
{
    public CaptchaServiceMock(ServiceCollection services)
    {
        services.RemoveService<ICaptchaService>();
        services.AddTransient(_ => Mock.Object);

        Reset();
    }

    public Mock<ICaptchaService> Mock { get; private set; } = default!;

    public void Reset()
    {
        Mock = new();
        Mock.Setup(x => x.ValidateAsync(It.IsAny<string>())).ReturnsAsync(true);
    }
}