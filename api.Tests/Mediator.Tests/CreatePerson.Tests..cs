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
public class CreatePersonTests
{
    private SqliteConnection _connection;
    private DbContextOptions<StargateContext> _contextOptions;
    private CreatePersonHandler _createPersonHandler;

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
    [NUnit.Framework.Description("CreatePerson should create a record in the DB")]
    public async Task CreatePerson_MakesRecord()
    {
        var db = CreateContext();
        _createPersonHandler = new CreatePersonHandler(db);

        var request = new CreatePerson { Name = "TestUser" };
        var cancelToken = new CancellationToken();

        var results = await _createPersonHandler.Handle(request, cancelToken);

        Assert.That(results, Is.Not.Null);
    }

    #endregion
}