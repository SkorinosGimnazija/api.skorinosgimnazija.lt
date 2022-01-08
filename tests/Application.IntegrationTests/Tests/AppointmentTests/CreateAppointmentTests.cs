namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AppointmentTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

[Collection("App")]
public class CreateAppointmentTests
{
    private readonly AppFixture _app;

    public CreateAppointmentTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetDatabase();
    }


}
