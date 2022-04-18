using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;
using AsyncDapperExtensions;
using Microsoft.Data.SqlClient;

namespace DapperGenericRepository;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private static IEnumerable<string> _listOfProperties;
    private static string _selectFields = string.Empty;
    private readonly string _connectionString;
    private readonly string _tableName;

    static GenericRepository()
    {
        _listOfProperties = GenerateListOfProperties(typeof(T).GetProperties());
    }

    protected GenericRepository(string connectionString, string tableName)
    {
        _connectionString = connectionString;
        _tableName = tableName;
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await LoadSelectFields(async () =>
        {
            using var connection = CreateConnection();
            return await connection.QueryAsyncWithToken<T>($"SELECT {_selectFields} FROM {_tableName}", cancellationToken: cancellationToken);
        });
    }

    public Task<T> GetAsync(object id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task InsertAsync(T t, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #region PrivateHelperMethods

    private SqlConnection SqlConnection()
    {
        return new SqlConnection(_connectionString);
    }

    /// <summary>
    /// Open new connection and return it for use
    /// </summary>
    /// <returns></returns>
    private IDbConnection CreateConnection()
    {
        var conn = SqlConnection();
        conn.Open();
        return conn;
    }
    
    private TU LoadSelectFields<TU>(Func<TU> method)
    {
        if (string.IsNullOrEmpty(_selectFields)) _selectFields = GenerateSelectFields(_listOfProperties);
        return method.Invoke();
    }
    private static IEnumerable<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
    {
        return (from prop in listOfProperties
            let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
            where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
            select prop.Name).ToList();
    }
    
    private static string GenerateSelectFields(IEnumerable<string> properties)
    {
        var fields = new StringBuilder();
        foreach (var property in properties)
        {
            fields.Append($"{property},");
        }

        fields.Remove(fields.Length - 1, 1);
        return fields.ToString();
    }

    #endregion
}    
