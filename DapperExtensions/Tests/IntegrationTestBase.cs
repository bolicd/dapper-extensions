using System;
using Infrastructure.Infrastructure;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace Tests;

[SetUpFixture]
public abstract class IntegrationTestBase
{
    public string ConnectionString { get; set; } = string.Empty;
    private string DatabaseName { get; set; } = string.Empty;

    [SetUp]
    public void StartDatabase()
    {
        DatabaseName = Guid.NewGuid().ToString();
        ConnectionString = $@"Server=(localdb)\mssqllocaldb;Database={DatabaseName};Trusted_Connection=True;";
        DbMigrationLite.ExecuteMigration(ConnectionString);
    }

    //TODO: Why is teardown not working
    [TearDown]
    public void Teardown()
    {
        
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
    
        using (var command = new SqlCommand())
        {
            command.Connection = conn;
    
            command.CommandText = $"USE master";
            command.ExecuteNonQuery();
            
            command.CommandText = $"alter database \"{DatabaseName}\" set single_user with rollback immediate";
            command.ExecuteNonQuery();
    
            command.CommandText = $"DROP DATABASE \"{DatabaseName}\"";
            command.ExecuteNonQuery();
        }
    
        conn.Close();
    }
}