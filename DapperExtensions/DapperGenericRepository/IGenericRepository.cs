using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DapperGenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> GetAsync(object id, CancellationToken cancellationToken = default);
        Task InsertAsync(T t, CancellationToken cancellationToken = default);
        void InsertBulk(IEnumerable<T> items);
        Task UpdateAsync(T t, CancellationToken cancellationToken = default);
        Task DeleteAsync(object id, CancellationToken cancellationToken = default);
        Task InsertRangeAsync(IEnumerable<T> t, CancellationToken cancellationToken = default);
    }
}