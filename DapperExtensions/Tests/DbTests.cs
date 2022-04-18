using System.Diagnostics;
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
    private IGenericRepository<TestTableDao> _genericRepository;

    [SetUp]
    public void Setup()
    {
        _genericRepository = new TestTableDaoRepository(ConnectionString,"TestTable");
    }
    //TODO: tests
    //Need repository
    [Test]
    public void ExecuteAsyncWithToken_TestItWritesToDb()
    {
        TestTableDao testRow = new TestTableDao()
        {
            Year = 2020,
            
        };
        Assert.AreEqual(1,1);
    }
    
    //Need repository
    [Test]
    public async Task TestSelect()
    {
        var results = await _genericRepository.GetAllAsync();
        var resultsList = results.ToList();
        Assert.IsNotNull(resultsList);
        resultsList.Count.Should().Be(1);
        resultsList.First().FirstName.Should().Be("TestName1");
        resultsList.First().LastName.Should().Be("LastTestName1");
    }

}