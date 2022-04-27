# Generic Repository

Generic repository based on dapper exposes most common repository methods as well as providing some additional functionality.

Here is the complete interface that is exposed:

```cs
public interface IGenericRepository<T> where T: class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> GetAsync(object id,CancellationToken cancellationToken = default);
    Task InsertAsync(T t,CancellationToken cancellationToken = default);
    void InsertBulk(IEnumerable<T> items); 
    Task UpdateAsync(T t, CancellationToken cancellationToken = default);
    Task DeleteAsync(object id, CancellationToken cancellationToken = default);
    Task InsertRangeAsync(IEnumerable<T> t, CancellationToken cancellationToken = default);   
}
```

Each async method also supports cancellation token except for InsertBulk which is not async and is using SqlBulkCopy for efficient insert large number of records without a transaction.

Repository generates queries based on provided generic type T and usage is pretty straightforward. Examples can be found in Tests project.

Please note:
For inserting large number of records use InsertBulk for inserting large number of records with the transaction use InsertRangeAsync.


# Dapper-extensions
A few dapper extension that can help in everyday use

# How-to-use

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



