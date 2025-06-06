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
public class PersonControllerTests
{
    private PersonController _controller;
    private Mock<IMediator> _mediator;
    private Mock<ILogService> _logService;

    [SetUp]
    public void SetUp()
    {

        _mediator = new Mock<IMediator>();
        _logService = new Mock<ILogService>();
        _controller = new PersonController(_mediator.Object,_logService.Object);
    }

    [Test]
    [NUnit.Framework.Description("Test that GetPeople calls correctly")]
    public void GetPeople_CallsMediator_CallsLogger()
    {
        var mockData = new GetPeopleResult();
        List<PersonAstronaut> people = new List<PersonAstronaut>{};
        PersonAstronaut person1 = new PersonAstronaut { PersonId = 1, Name = "TestUser", CurrentRank = "1LT" };
        PersonAstronaut person2 = new PersonAstronaut { PersonId = 2, Name = "TestUser2", CurrentRank = "1LT" };
        people.AddRange(person1, person2);
        mockData.People = people;

        _mediator.Setup(m => m.Send(It.IsAny<GetPeople>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockData);
        _logService.Setup(l => l.Log(It.IsAny<string>())).Returns(Task.FromResult(1));

        var result = _controller.GetPeople();

        _mediator.Verify(m => m.Send(It.IsAny<GetPeople>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
        _logService.Verify(l => l.Log(It.IsAny<string>()), Times.Exactly(1));
    }

    [Test]
    [NUnit.Framework.Description("Test that GetPeople logs on failure")]
    public void GetPeople_CallsLoggerOnFailure()
    {
        var mockData = new GetPeopleResult();
        List<PersonAstronaut> people = new List<PersonAstronaut> { };
        PersonAstronaut person1 = new PersonAstronaut { PersonId = 1, Name = "TestUser", CurrentRank = "1LT" };
        PersonAstronaut person2 = new PersonAstronaut { PersonId = 2, Name = "TestUser2", CurrentRank = "1LT" };
        people.AddRange(person1, person2);
        mockData.People = people;

        var exception = new Exception("Cannot fetch data");

        _mediator.Setup(m => m.Send(It.IsAny<GetPeople>(), It.IsAny<CancellationToken>())).Throws(exception);
        _logService.Setup(l => l.Log(It.IsAny<string>())).Returns(Task.FromResult(1));


        var result = _controller.GetPeople();

        _mediator.Verify(m => m.Send(It.IsAny<GetPeople>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
        _logService.Verify(l => l.Log(exception.ToString()), Times.Exactly(1));
    }
}
