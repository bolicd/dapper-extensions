using System.Collections.Generic;
using DapperGenericRepository.Helpers;
using FluentAssertions;
using Infrastructure.Models;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class HelpersTests
{
    [Test]
    public void DataTable_Generated_For_GivenType()
    {
        //GIVEN
        var listOfModels = new List<TestTableDao>()
        {
            new()
            {
                Id = 1,
                Year = 2022,
                FirstName = "d",
                LastName = "b"
            },
            new()
            {
                Id = 2,
                Year = 2022,
                FirstName = "d",
                LastName = "b"
            }
        };

        //WHEN
        var dataTable = BulkInsertHelpers.CreateDataTable(listOfModels);
        
        //THEN
        dataTable.Columns.Count.Should().Be(4);
    }
}