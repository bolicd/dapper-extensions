using Infrastructure.Models;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class DbTests : IntegrationTestBase
{
    
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
    
}