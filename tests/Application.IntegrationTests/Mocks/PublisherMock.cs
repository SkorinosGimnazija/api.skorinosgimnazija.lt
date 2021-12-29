﻿namespace SkorinosGimnazija.Application.IntegrationTests.Mocks;

using Microsoft.Extensions.DependencyInjection;
using Moq;
using SkorinosGimnazija.Application.Common.Interfaces;
using SkorinosGimnazija.Application.IntegrationTests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

public class PublisherMock
{
    public Mock<IPublisher> Mock { get; } = new();

    public PublisherMock(ServiceCollection services)
    {
        services.RemoveService<IPublisher>();
        services.AddTransient(_ => Mock.Object);
    }
}