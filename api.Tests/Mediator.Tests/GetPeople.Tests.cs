using api.Tests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;
using System.Reflection.Metadata;

namespace api.Tests.Mediator.Tests;

[TestFixture]
public class GetPeopleTests
{
    private SqliteConnection _connection;
    private DbContextOptions<StargateContext> _contextOptions;
    private GetPeopleHandler _getPeopleHandler;

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

        //if (context.Database.EnsureCreated())
        //{
        //    //using var viewCommand = context.Database.GetDbConnection().CreateCommand();
        //    //viewCommand.CommandText = @"
        //    //   CREATE TABLE Person (
	       //    // Id INTEGER PRIMARY KEY,
	       //    // Name TEXT NOT NULL,
	       //    // AstronautDetail TEXT,
	       //    // AstronautDuty TEXT
        //    //);";
        //    //viewCommand.ExecuteNonQuery();
        //}

        ////context.People.Add(new Person { Id = 1, Name = "Brad" });

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
    [NUnit.Framework.Description("GetPeople should return a list of people")]
    public async Task GetPeople_ReturnsData()
    {
        var db = CreateContext();
        _getPeopleHandler = new GetPeopleHandler(db);

        var request = new GetPeople();
        var cancelToken = new CancellationToken();

        var results = await _getPeopleHandler.Handle(request, cancelToken);

        Assert.That(results, Is.Not.Null);
        Assert.That(results.People.Count, Is.EqualTo(2));
    }

    #endregion
}