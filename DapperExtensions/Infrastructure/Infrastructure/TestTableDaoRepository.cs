using DapperGenericRepository;
using Infrastructure.Models;

namespace Infrastructure.Infrastructure;

public class TestTableDaoRepository: GenericRepository<TestTableDao>
{
    public TestTableDaoRepository(string connectionString, string tableName) : base(connectionString, tableName)
    {
    }
}