namespace SkorinosGimnazija.Application.IntegrationTests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Moq;

using SkorinosGimnazija.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;

public class CaptchaServiceMock
{
    public Mock<ICaptchaService> Mock { get; } = new();

    public CaptchaServiceMock(ServiceCollection services)
    {
        services.RemoveService<ICaptchaService>();
        services.AddTransient(_ => Mock.Object);
      
        Mock.Setup(x => x.ValidateAsync(It.IsAny<string>())).ReturnsAsync(true);
    }
}
