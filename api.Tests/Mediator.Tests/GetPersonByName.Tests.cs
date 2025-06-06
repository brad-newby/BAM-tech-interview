using api.Tests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;
using System.Reflection.Metadata;

namespace api.Tests.Mediator.Tests;

[TestFixture]
public class GetPersonByNameTests
{
    private SqliteConnection _connection;
    private DbContextOptions<StargateContext> _contextOptions;
    private GetPersonByNameHandler _getPersonByNameHandler;

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
    [NUnit.Framework.Description("GetPersonByName should a single person")]
    public async Task GetPersonByName_ReturnsData()
    {
        var db = CreateContext();
        _getPersonByNameHandler = new GetPersonByNameHandler(db);

        var request = new GetPersonByName { Name = "John Doe" };
        var cancelToken = new CancellationToken();

        var results = await _getPersonByNameHandler.Handle(request, cancelToken);

        Assert.That(results, Is.Not.Null);
        Assert.That(results.Person!.Name, Is.EqualTo("John Doe"));
        Assert.That(results.Person!.CurrentRank, Is.EqualTo("1LT"));
    }

    #endregion
}