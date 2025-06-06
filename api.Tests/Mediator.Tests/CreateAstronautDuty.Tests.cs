using api.Tests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;
using System.Reflection.Metadata;

namespace api.Tests.Mediator.Tests;

[TestFixture]
public class CreateAstronautDutyTests
{
    private SqliteConnection _connection;
    private DbContextOptions<StargateContext> _contextOptions;
    private CreateAstronautDutyHandler _createAstronautDutyHandler;

    [SetUp]
    public void setup()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<StargateContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new StargateContext(_contextOptions);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        context.SaveChanges();
    }

    StargateContext CreateContext() => new StargateContext(_contextOptions);

    [TearDown]
    public void Dispose()
    {
        _connection.Dispose();
    }

    #region Happy Path

    [Test]
    [NUnit.Framework.Description("CreateAstronautDuty should create a record in the DB")]
    public async Task CreateAstronautDuty_MakesRecord()
    {
        var db = CreateContext();
        _createAstronautDutyHandler = new CreateAstronautDutyHandler(db);

        var request = new CreateAstronautDuty { Name = "John Doe", Rank = "2LT", DutyTitle = "Programmer", DutyStartDate = DateTime.Now };
        var cancelToken = new CancellationToken();

        var results = await _createAstronautDutyHandler.Handle(request, cancelToken);

        Assert.That(results, Is.Not.Null);
    }

    #endregion
}