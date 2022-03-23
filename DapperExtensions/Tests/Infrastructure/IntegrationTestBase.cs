using System;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;

namespace Tests.Infrastructure;

[SetUpFixture]
public abstract class IntegrationTestBase
{
    private string ConnectionString { get; set; } = string.Empty;
    private string DatabaseName { get; set; } = string.Empty;

    [OneTimeSetUp]
    public void StartDatabase()
    {
        DatabaseName = Guid.NewGuid().ToString();
        ConnectionString = $@"Server=(localdb)\mssqllocaldb;Database={DatabaseName};Trusted_Connection=True;";
        DbMigrationLite.ExecuteMigration(ConnectionString);
    }

    //TODO: Why is teardown not working
    // [OneTimeTearDown]
    // public void Teardown()
    // {
    //     
    //     using var conn = new SqlConnection(ConnectionString);
    //     conn.Open();
    //
    //     using (var command = new SqlCommand())
    //     {
    //         command.Connection = conn;
    //
    //         command.CommandText = "USE master";
    //         command.ExecuteNonQuery();
    //
    //         command.CommandText = $"DROP DATABASE \"{_databaseName}\"";
    //         command.ExecuteNonQuery();
    //     }
    //
    //     conn.Close();
    // }
}