namespace DapperGenericRepository;

public interface IGenericRepository<T> where T: class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> GetAsync(object id,CancellationToken cancellationToken = default);
    Task InsertAsync(T t,CancellationToken cancellationToken = default);
}