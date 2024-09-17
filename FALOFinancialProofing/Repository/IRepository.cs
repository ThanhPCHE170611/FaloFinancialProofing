using FALOFinancialProofing.Core;
using System.Linq.Expressions;

namespace FALOFinancialProofing.Repository
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> Get(long id);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null);
        Task<IEnumerable<T>> GetAllIncludeDeleted(Expression<Func<T, bool>> filter = null);
        Task<T> GetFirstItem(Expression<Func<T, bool>> filter);
        Task<T> InsertAsync(T entity);
        Task<bool> InsertManyAsync(IEnumerable<T> entities);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(long id);
    }
}
