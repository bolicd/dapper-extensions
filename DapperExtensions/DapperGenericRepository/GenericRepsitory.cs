using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;
using AsyncDapperExtensions;
using DapperGenericRepository.Helpers;
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
        return await LoadFields(async () =>
        {
            using var connection = CreateConnection();
            return await connection.QueryAsyncWithToken<T>($"SELECT {_selectFields} FROM {_tableName}",
                cancellationToken: cancellationToken);
        }, true);
    }

    public async Task<T> GetAsync(object id, CancellationToken cancellationToken = default)
    {
        return await LoadFields(async () =>
        {
            using var connection = CreateConnection();
            var result =
                await connection.QuerySingleOrDefaultWithToken<T>($"SELECT * FROM {_tableName} WHERE Id=@Id",
                    new { Id = id }, cancellationToken: cancellationToken);
            if (result == null) throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");

            return result;
        }, true);
    }

    public async Task InsertAsync(T t, CancellationToken cancellationToken = default)
    {
        var insertQuery = GenerateInsertQuery();

        using var connection = CreateConnection(); 
        var result = await connection.ExecuteAsyncWithToken(insertQuery, t,cancellationToken: cancellationToken).ConfigureAwait(false);
        Console.Write(result);
    }

    public void InsertBulk(IEnumerable<T> items)
    {
        var dataTable = BulkInsertHelpers.CreateDataTable(items);
        using var bulkInsert = new SqlBulkCopy(_connectionString);
        bulkInsert.DestinationTableName = _tableName;
        bulkInsert.WriteToServer(dataTable);
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

    private TU LoadFields<TU>(Func<TU> method, bool includeId = false)
    {
        if (string.IsNullOrEmpty(_selectFields)) _selectFields = GenerateSelectFields(_listOfProperties, includeId);
        return method.Invoke();
    }

    private static IEnumerable<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
    {
        return (from prop in listOfProperties
            let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
            where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
            select prop.Name).ToList();
    }

    private void IgnoreId(string property, Action action)
    {
        if (!property.Equals("Id")) action.Invoke();
    }

    private string GenerateSelectFields(IEnumerable<string> properties, bool includeId)
    {
        var fields = new StringBuilder();
        foreach (var property in properties)
        {
            if (includeId == false)
            {
                IgnoreId(property, () => { fields.Append($"{property},"); });
            }
            else
            {
                fields.Append($"{property},");
            }
        }

        fields.Remove(fields.Length - 1, 1);
        return fields.ToString();
    }

    private string GenerateInsertQuery()
    {
        var insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");

        insertQuery.Append("(");
        foreach (var listOfProperty in _listOfProperties)
        {
            //TODO: extract this check
            IgnoreId(listOfProperty, () => { insertQuery.Append($"[{listOfProperty}],"); });
        }

        insertQuery.Remove(insertQuery.Length - 1, 1).Append(") VALUES (");

        foreach (var listOfProperty in _listOfProperties)
        {
            IgnoreId(listOfProperty, () => { insertQuery.Append($"@{listOfProperty},"); });
        }

        insertQuery.Remove(insertQuery.Length - 1, 1).Append(")");

        return insertQuery.ToString();
    }

    #endregion
}