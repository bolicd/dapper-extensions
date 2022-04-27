using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperGenericRepository;
using FluentAssertions;
using Infrastructure.Infrastructure;
using Infrastructure.Models;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class DbTests : IntegrationTestBase
{
    private IGenericRepository<TestTableDao> _genericRepository = null!;

    [SetUp]
    public void Setup()
    {
        _genericRepository = new TestTableDaoRepository(ConnectionString, "TestTable");
    }

    [Test]
    public async Task ExecuteAsyncWithToken_TestItWritesToDb()
    {
        //GIVEN
        var testRow = new TestTableDao() { Year = 2020, FirstName = "test", LastName = "test" };

        //WHEN
        await _genericRepository.InsertAsync(testRow);

        //THEN
        var results = await _genericRepository.GetAllAsync();
        var resultsList = results.ToList();
        resultsList.Count(x => x.FirstName == "test" && x.LastName == "test").Should().Be(1);
        resultsList.Count.Should().Be(2);
    }
    
    [Test]
    public async Task TestItUpdatesDb()
    {
        //GIVEN
        var testRow = new TestTableDao() { Year = 2020, FirstName = "test", LastName = "test" };
        await _genericRepository.InsertAsync(testRow);
        var results = await _genericRepository.GetAllAsync();
        var resultsList = results.ToList();
        var id = resultsList.Single(x => x.FirstName == "test" && x.LastName == "test").Id;
        
        //WHEN
        var updateRow = new TestTableDao { Id = id, Year = 2021, FirstName = "test2", LastName = "test2" };
        await _genericRepository.UpdateAsync(updateRow);
        

        //THEN
        var r = await _genericRepository.GetAllAsync();
        var resultUpdate = r.Single(x => x.Id == id);
        resultUpdate.FirstName.Should().Be("test2");
    }

    [Test]
    public async Task TestSelect()
    {
        var results = await _genericRepository.GetAllAsync();
        var resultsList = results.ToList();
        resultsList.Count(x => x.FirstName == "TestName1" && x.LastName == "LastTestName1").Should().Be(1);
        resultsList.Single(x => x.FirstName == "TestName1" && x.LastName == "LastTestName1").Id.Should().NotBe(0);
    }
    
    [Test]
    public async Task TestSelectById()
    {
        var results = await _genericRepository.GetAllAsync();
        var resultsList = results.ToList();
        var id = resultsList.Single(x => x.FirstName == "TestName1" && x.LastName == "LastTestName1").Id;
        var item = await _genericRepository.GetAsync(id);
        item.Should().NotBeNull();
        item.Id.Should().Be(id);
        item.Year.Should().Be(2022);
        item.FirstName.Should().Be("TestName1");
        item.LastName.Should().Be("LastTestName1");
    }

    [Test]
    public async Task BulkInsert_Test()
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
        _genericRepository.InsertBulk(listOfModels);
        
        //THEN
        var results = await _genericRepository.GetAllAsync();
        var resultsList = results.ToList();
        resultsList.Count.Should().Be(3);
    }
    
    [Test]
    public async Task InsertRange_Test()
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
        await _genericRepository.InsertRangeAsync(listOfModels);
        
        //THEN
        var results = await _genericRepository.GetAllAsync();
        var resultsList = results.ToList();
        resultsList.Count.Should().Be(3);
    }
    
    [Test]
    public async Task TestDelete()
    {
        //GIVEN
        var testRow = new TestTableDao() { Year = 2020, FirstName = "test", LastName = "test" };
        await _genericRepository.InsertAsync(testRow);
        var results = await _genericRepository.GetAllAsync();
        var resultsList = results.ToList();
        var id = resultsList.Single(x => x.FirstName == "test" && x.LastName == "test").Id;
        
        //WHEN
        await _genericRepository.DeleteAsync(id);

        //THEN
        var r = await _genericRepository.GetAllAsync();
        var resultUpdate = r.SingleOrDefault(x => x.Id == id);
        resultUpdate.Should().BeNull();
    }
}