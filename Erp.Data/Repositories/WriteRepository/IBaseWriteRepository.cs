using Erp.Data.Interfaces;
using System.Linq.Expressions;

namespace Erp.Data.Repositories.WriteRepositories
{
    public interface IBaseWriteRepository<TEntity, TKeyType> where TEntity : IEntityBase<TKeyType>
    {
        Task<TEntity> GetByIdAsync(TKeyType id);
        Task AddAsync(TEntity entity, bool addLog = true);
        Task AddRangeAsync(List<TEntity> entities, bool addLog = true);
        Task UpdateAsync(TEntity entity, Expression<Func<TEntity, object>>[]? properties = null, bool addLog = true);
        Task UpdateRangeAsync(List<TEntity> entities, Expression<Func<TEntity, object>>[]? properties = null, bool addLog = true);
        Task DeleteAsync(TEntity entity, bool addLog = true);
        Task DeleteRangeAsync(List<TEntity> entities, bool addLog = true);
        Task<List<TEntity>> FindTask(Expression<Func<TEntity, bool>> expression);
        Task<List<TResult>> FindTask<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selector);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> expression);
        Task<bool> IsExist(Expression<Func<TEntity, bool>> expression);
        Task<int> Count(Expression<Func<TEntity, bool>>? expression = null);
        Task<int> Sum(Expression<Func<TEntity, int>> property, Expression<Func<TEntity, bool>>? expression = null);
        Task<int?> Sum(Expression<Func<TEntity, int?>> property, Expression<Func<TEntity, bool>>? expression = null);
        Task<int> Max(Expression<Func<TEntity, int>> property, Expression<Func<TEntity, bool>>? expression = null);
    }
}