using NUnit.Framework;
using Tests.Infrastructure;

namespace Tests;

[TestFixture]
public class DbTests : IntegrationTestBase
{
    [Test]
    public void TestThis()
    {
        Assert.AreEqual(1,1);
    }
    
}