using Core.Utilities;
using Erp.Data.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Erp.Data.Repositories.WriteRepositories
{
    public class BaseWriteRepository<TEntity, TKeyType> : IBaseWriteRepository<TEntity, TKeyType> where TEntity : class, IEntityBase<TKeyType>
    {
        protected DbContext _context;
        protected IQueryable<TEntity> _dbQuery;
        private readonly DbSet<TEntity> _dbSet;

        public BaseWriteRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _dbQuery = _dbSet.AsNoTracking();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            try
            {
                entity.CreatedByUserId = Convert.ToInt32(CurrentUser.UserIdentity?.Name);
            }
            catch (Exception)
            {
                // ignored
            }

            entity.RegistrationDate = DateTime.Now;
            entity.UpdatingDate = null;

            _dbSet.Add(entity);
            await SaveChangesAsync();
        }

        public virtual async Task AddRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.RegistrationDate = DateTime.Now;
                entity.UpdatingDate = null;
                try
                {
                    entity.CreatedByUserId = Convert.ToInt32(CurrentUser.UserIdentity?.Name);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            _dbSet.AddRange(entities);
            await SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (IsAttach(entity))
            {
                try
                {
                    _dbSet.Attach(entity);
                }
                catch (Exception)
                {

                }
            }

            try
            {
                _dbSet.Remove(entity);
                await SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Data is Changed or Not Available");
            }
            catch (DbUpdateException)
            {
                throw new Exception("This Data Can't Delete Because It is Using Elsewhere");
            }
            catch (Exception)
            {
                throw new Exception("Unexpected Error");
            }
        }

        public virtual async Task DeleteRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (IsAttach(entity))
                {
                    try
                    {
                        _dbSet.Attach(entity);
                    }
                    catch (Exception)
                    {

                    }
                    _dbSet.Remove(entity);
                }
            }
            try
            {
                await SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Data is Changed or Not Available");
            }
            catch (DbUpdateException)
            {
                throw new Exception("This Data Can't Delete Because It is Using Elsewhere");
            }
            catch (Exception)
            {
                throw new Exception("Unexpected Error");
            }
        }
        public virtual async Task<TEntity> GetByIdAsync(TKeyType id)
        {
            var entity = await _dbSet.FindAsync(id);

            return entity;
        }

        public virtual async Task UpdateAsync(TEntity entity, Expression<Func<TEntity, object>>[]? properties = null)
        {
            entity.UpdatingDate = DateTime.Now;

            try
            {
                entity.UpdatedByUserId = Convert.ToInt32(CurrentUser.UserIdentity?.Name);
            }
            catch (Exception)
            {
                // ignored
            }

            if (IsAttach(entity))
            {
                try
                {
                    _dbSet.Attach(entity);
                }
                catch (Exception)
                {

                }

            }

            if (properties != null && properties.Any())
            {
                foreach (var item in properties)
                    _context.Entry(entity).Property(item).IsModified = true;
                _context.Entry(entity).Property(x => x.UpdatingDate).IsModified = true;
                _context.Entry(entity).Property(x => x.UpdatedByUserId).IsModified = true;
            }
            else
            {
                _context.Entry(entity).State = EntityState.Modified;
            }

            _context.Entry(entity).Property(x => x.CreatedByUserId).IsModified = false;
            _context.Entry(entity).Property(x => x.RegistrationDate).IsModified = false;

            try
            {
                await SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Data is Changed or Not Available");
            }
            catch (Exception)
            {
                throw new Exception("Unexpected Error");
            }
        }

        public virtual async Task UpdateRangeAsync(List<TEntity> entities, Expression<Func<TEntity, object>>[]? properties = null)
        {
            foreach (var entity in entities)
            {
                entity.UpdatingDate = DateTime.Now;

                try
                {
                    entity.UpdatedByUserId = Convert.ToInt32(CurrentUser.UserIdentity?.Name);
                }
                catch (Exception)
                {
                    // ignored
                }

                if (IsAttach(entity))
                {
                    try
                    {
                        _dbSet.Attach(entity);
                    }
                    catch (Exception)
                    {

                    }

                }

                if (properties != null && properties.Any())
                {
                    foreach (var item in properties)
                        _context.Entry(entity).Property(item).IsModified = true;
                    _context.Entry(entity).Property(x => x.UpdatingDate).IsModified = true;
                    _context.Entry(entity).Property(x => x.UpdatedByUserId).IsModified = true;
                }
                else
                {
                    _context.Entry(entity).State = EntityState.Modified;
                }

                _context.Entry(entity).Property(x => x.CreatedByUserId).IsModified = false;
                _context.Entry(entity).Property(x => x.RegistrationDate).IsModified = false;
            }

            try
            {
                await SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Data is Changed or Not Available");
            }
            catch (Exception)
            {
                throw new Exception("Unexpected Error");
            }
        }

        public virtual async Task<List<TEntity>> FindTask(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbQuery.Where(expression).ToListAsync();
        }

        public virtual async Task<List<TResult>> FindTask<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selector)
        {
            return await _dbQuery.Where(expression).Select(selector).ToListAsync();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbQuery.FirstOrDefaultAsync(expression);
        }

        public virtual async Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbQuery.LastOrDefaultAsync(expression);
        }

        public async Task<bool> IsExist(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbQuery.AnyAsync(expression);
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>>? expression = null)
        {
            return await _dbQuery.CountAsync(expression);
        }

        private bool IsAttach(TEntity entity)
        {
            return _context.Entry(entity).State == EntityState.Detached;
        }

        private async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> Sum(Expression<Func<TEntity, int>> property, Expression<Func<TEntity, bool>>? expression = null)
        {
            if (expression != null) return await _dbQuery.Where(expression).SumAsync(property);
            return await _dbQuery.SumAsync(property);
        }

        public virtual async Task<int?> Sum(Expression<Func<TEntity, int?>> property, Expression<Func<TEntity, bool>>? expression = null)
        {
            if (expression != null) return await _dbQuery.Where(expression).SumAsync(property);
            return await _dbQuery.SumAsync(property);
        }

        public virtual async Task<int> Max(Expression<Func<TEntity, int>>? property, Expression<Func<TEntity, bool>>? expression = null)
        {
            if (expression != null) return await _dbQuery.Where(expression).MaxAsync(property);
            return await _dbQuery.MaxAsync(property);
        }

    }
}