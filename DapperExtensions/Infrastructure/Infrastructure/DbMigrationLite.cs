using System.Reflection;
using DbUp;

namespace Infrastructure.Infrastructure;

public class DbMigrationLite
{
    /// <summary>
    /// Creates database using given SQL scripts, for testing purposes
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static int ExecuteMigration(string connectionString)
    {
        Console.WriteLine("Migration Starting");
        EnsureDatabase.For.SqlDatabase(connectionString);
        var upgradeEngineBuilder = DeployChanges.To
            .SqlDatabase(connectionString, null)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .WithTransaction()
            .LogToConsole();
        
        var upgrader = upgradeEngineBuilder.Build();

        upgrader.GetDiscoveredScripts().ForEach(x => Console.WriteLine(x.Name));
        if (upgrader.IsUpgradeRequired())
        {
            var results = upgrader.PerformUpgrade();
            if (results.Successful)
                Console.WriteLine("Database upgrade success");
            else
            {
                Console.WriteLine($"Failed {results.Error}");
                return -1;
            }
        }
        else
        {
            Console.WriteLine("No upgrade is required");
        }

        return 0;
    }
}