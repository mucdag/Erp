using Erp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Erp.Data.Repositories.WriteRepositories
{
    public interface IBaseWriteRepository<TEntity, TKeyType> where TEntity : IEntityBase<TKeyType>
    {
        Task<TEntity> GetByIdAsync(TKeyType id);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        Task UpdateAsync(TEntity entity, Expression<Func<TEntity, object>>[]? properties = null);
        Task UpdateRangeAsync(List<TEntity> entities, Expression<Func<TEntity, object>>[]? properties = null);
        Task DeleteAsync(TEntity entity);
        Task DeleteRangeAsync(List<TEntity> entities);
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