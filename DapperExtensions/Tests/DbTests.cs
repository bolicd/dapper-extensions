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
}