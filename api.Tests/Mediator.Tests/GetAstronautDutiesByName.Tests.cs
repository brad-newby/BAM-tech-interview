using api.Tests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;
using System.Reflection.Metadata;

namespace api.Tests.Mediator.Tests;

[TestFixture]
public class GetAstronautDutiesByNameTests
{
    private SqliteConnection _connection;
    private DbContextOptions<StargateContext> _contextOptions;
    private GetAstronautDutiesByNameHandler _getAstronautDutiesByNameHandler;

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
    [NUnit.Framework.Description("GetAstronautDutiesByName should return a list of duties for a person")]
    public async Task GetAstronautDutiesByName_ReturnsData()
    { 
        var db = CreateContext();
        _getAstronautDutiesByNameHandler = new GetAstronautDutiesByNameHandler(db);

        var request = new GetAstronautDutiesByName { Name = "john doe" };
        var cancelToken = new CancellationToken();

        var results = await _getAstronautDutiesByNameHandler.Handle(request, cancelToken);

        //Instead of these multiple asserts we could compare results to the seed data directly if we where seeding in this file or if the seed data was its own object.
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Person, Is.Not.Null);
        Assert.That(results.AstronautDuties, Is.Not.Null);
        Assert.That(results.Person!.Name, Is.EqualTo("John Doe"));
        Assert.That(results.AstronautDuties.Count, Is.EqualTo(1));
    }

    #endregion
}