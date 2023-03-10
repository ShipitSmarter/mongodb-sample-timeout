using System.Diagnostics;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Sample;

if (args.Length != 1)
{
    Console.WriteLine("Please provide the connectionString for you MongoDB Atlas server.");
    return;
}

var connectionString = args[0];

var databaseName = "exampledb";
var collectionName = "collection1";

using var loggerFactory = LoggerFactory.Create(b =>
{
    b.AddSimpleConsole();
    b.SetMinimumLevel(LogLevel.Debug);
});

var settings = MongoClientSettings.FromConnectionString(connectionString);
settings.LoggingSettings = new LoggingSettings(loggerFactory);
settings.ServerApi = new ServerApi(ServerApiVersion.V1);

var mongoClient = new MongoClient(settings);

var database = mongoClient.GetDatabase(databaseName);

while (true)
{
    try
    {
        var sw = Stopwatch.StartNew();

        var entity = new Entity
        {
            CreatedOn = DateTime.UtcNow,
            Data = new Upload
            {
                Filename = "testing123.txt",
                Status = "New",
                BlobReference = Guid.NewGuid(),
                ConnectionId = "conn-1234"
            }
        };
        
        Console.WriteLine();
        Console.WriteLine(DateTime.UtcNow);
        var collection = database.GetCollection<Entity>(collectionName);
        Console.WriteLine($"Inserting with ref {entity.Data.BlobReference}");
        await collection.InsertOneAsync(entity);
        Console.WriteLine($"Inserted with ref {entity.Data.BlobReference}");
        sw.Stop();
        Console.WriteLine($"Completed in {sw.ElapsedMilliseconds}ms");

        if (sw.ElapsedMilliseconds > 5000)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("!!! ALERT TAKING TO LONG !!!");
            break;
        }

        await Task.Delay(TimeSpan.FromMinutes(5));
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"Exception occured, {ex.Message}\n{ex.StackTrace}");
        break;
    }
}