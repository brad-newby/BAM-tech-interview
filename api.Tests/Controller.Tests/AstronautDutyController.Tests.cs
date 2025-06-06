using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateAPI.Business.Queries;
using StargateAPI.Controllers;
using StargateAPI.Interface;
using System.ComponentModel;
using System.Data.SQLite;

namespace api.Tests.Controller.Tests;

[TestFixture]
public class AstronautDutyControllerTests
{
    private AstronautDutyController _controller;
    private Mock<IMediator> _mediator;
    private Mock<ILogService> _logService;

    [SetUp]
    public void SetUp()
    {

        _mediator = new Mock<IMediator>();
        _logService = new Mock<ILogService>();
        _controller = new AstronautDutyController(_mediator.Object, _logService.Object);
    }

    [Test]
    [NUnit.Framework.Description("Test that GetAstronautDutiesByName calls correctly")]
    public void GetAstronautDutiesByName_CallsMediator_CallsLogger()
    {
        var mockData = new GetAstronautDutiesByNameResult();
        List<AstronautDuty> astronautDuties = new List<AstronautDuty>();
        AstronautDuty duty1 = new AstronautDuty { Id = 1, PersonId = 1, Rank = "1LT", DutyTitle = "Commander", DutyStartDate = DateTime.Now };
        astronautDuties.Add(duty1);
        PersonAstronaut person1 = new PersonAstronaut { PersonId = 1, Name = "TestUser", CurrentRank = "1LT" };
        mockData.AstronautDuties = astronautDuties;
        mockData.Person = person1;


        _mediator.Setup(m => m.Send(It.IsAny<GetAstronautDutiesByName>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockData);
        _logService.Setup(l => l.Log(It.IsAny<string>())).Returns(Task.FromResult(1));

        var result = _controller.GetAstronautDutiesByName("TestUser");

        _mediator.Verify(m => m.Send(It.IsAny<GetAstronautDutiesByName>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
        _logService.Verify(l => l.Log(It.IsAny<string>()), Times.Exactly(1));
    }

    [Test]
    [NUnit.Framework.Description("Test that GetAstronautDutiesByName logs on failure")]
    public void GetAstronautDutiesByName_CallsLoggerOnFailure()
    {
        var mockData = new GetAstronautDutiesByNameResult();
        List<AstronautDuty> astronautDuties = new List<AstronautDuty>();
        AstronautDuty duty1 = new AstronautDuty { Id = 1, PersonId = 1, Rank = "1LT", DutyTitle = "Commander", DutyStartDate = DateTime.Now };
        astronautDuties.Add(duty1);
        PersonAstronaut person1 = new PersonAstronaut { PersonId = 1, Name = "TestUser", CurrentRank = "1LT" };
        mockData.AstronautDuties = astronautDuties;
        mockData.Person = person1;

        var exception = new Exception("Cannot find user");

        _mediator.Setup(m => m.Send(It.IsAny<GetAstronautDutiesByName>(), It.IsAny<CancellationToken>())).Throws(exception);
        _logService.Setup(l => l.Log(It.IsAny<string>())).Returns(Task.FromResult(1));


        var result = _controller.GetAstronautDutiesByName("SecondUser");

        _mediator.Verify(m => m.Send(It.IsAny<GetAstronautDutiesByName>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
        _logService.Verify(l => l.Log(exception.ToString()), Times.Exactly(1));
    }
}
