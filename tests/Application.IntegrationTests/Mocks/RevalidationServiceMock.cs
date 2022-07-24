namespace SkorinosGimnazija.Application.IntegrationTests.Mocks;

using Extensions;
using Infrastructure.Revalidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class RevalidationServiceMock
{
    public RevalidationServiceMock(ServiceCollection services)
    {
        services.RemoveService<IRevalidationService>();
        services.AddTransient(_ => Mock.Object);

        Mock.Setup(x => x.RevalidateAsync(It.IsAny<string>())).ReturnsAsync(true);

        Mock.Setup(x =>
                x.RevalidateAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()
                )
            )
            .ReturnsAsync(true);
    }

    public Mock<IRevalidationService> Mock { get; } = new();
}