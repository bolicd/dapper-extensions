# dapper-extensions
A few dapper extension that can help in everyday use

# how-to-use

Dapper can be used the same way. If you want to pass token to the method with the same name as dapperWithToken().
Example:

```cs
public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
{
  using (var connection = CreateConnection())
  {
      return await connection.QueryAsyncWithToken<T>($"SELECT * FROM {_tableName}", cancellationToken: cancellationToken);
  }
}
```



