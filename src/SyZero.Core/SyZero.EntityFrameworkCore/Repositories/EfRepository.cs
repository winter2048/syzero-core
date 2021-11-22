using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SyZero.Domain.Entities;
using SyZero.Domain.Repository;

namespace SyZero.EntityFrameworkCore.Repositories
{
    public class EfRepository<TDbContext, TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TDbContext : DbContext
    {
        protected TDbContext _dbContext;
        protected DbSet<TEntity> _dbSet;
        public EfRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }
        public EfRepository()
        {

        }

        #region Count
        public int Count(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.LongCount(where).ToInt32();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await _dbSet.LongCountAsync(where, cancellationToken)).ToInt32();
        }
        #endregion

        #region Insert
        public TEntity Add(TEntity entity)
        {
            var result = _dbSet.Add(entity);
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.SaveChanges();
            }
            return result.Entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var newEntity = (await _dbSet.AddAsync(entity, cancellationToken)).Entity;
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.SaveChanges();
            }
            return newEntity;
        }

        public int AddList(IQueryable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.SaveChanges();
            }
            return 1;
        }

        public async Task<int> AddListAsync(IQueryable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.SaveChanges();
            }
            return 1;
        }
        #endregion

        #region Delete
        public long Delete(long id)
        {
            var entity = GetModel(id);
            if (entity == null) return 0;
            _dbSet.Remove(entity);
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.SaveChanges();
            }
            return 1;
        }

        public long Delete(Expression<Func<TEntity, bool>> where)
        {
            var query = GetList(where);
            if (query == null || !query.Any()) return 0;
            _dbSet.RemoveRange(query);
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.SaveChanges();
            }
            return 1;
        }

        public async Task<long> DeleteAsync(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entity = await GetModelAsync(id);
            if (entity == null) return 0;
            _dbSet.Remove(entity);
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.SaveChanges();
            }
            return 1;
        }

        public async Task<long> DeleteAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            var list = await GetListAsync(where);
            if (list == null) return 0;
            _dbSet.RemoveRange(list);
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.SaveChanges();
            }
            return 1;
        }
        #endregion

        #region Select
        public IQueryable<TEntity> GetList()
        {
            return _dbSet.Where(p => true);
        }

        public IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.Where(where);
        }

        public async Task<IQueryable<TEntity>> GetListAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetListAsync(p => true, cancellationToken);
        }

        public async Task<IQueryable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetListAsync(where, cancellationToken);
        }

        public TEntity GetModel(long id)
        {
            return _dbSet.Find(id);
        }

        public async Task<TEntity> GetModelAsync(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbSet.FindAsync(id);

        }

        public TEntity GetModel(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.FirstOrDefault(where);
        }

        public async Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbSet.FirstOrDefaultAsync(where, cancellationToken);
        }

        public IQueryable<TEntity> GetPaged(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, bool isDesc = false)
        {
            if (isDesc)
                return _dbSet.OrderByDescending(sortBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return _dbSet.OrderBy(sortBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public async Task<IQueryable<TEntity>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, bool isDesc = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() => GetPaged(pageIndex, pageSize, sortBy, isDesc), cancellationToken);
        }

        public IQueryable<TEntity> GetPaged(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, Expression<Func<TEntity, bool>> where, bool isDesc = false)
        {
            if (isDesc)
                return _dbSet.Where(where).OrderByDescending(sortBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return _dbSet.Where(where).OrderBy(sortBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public async Task<IQueryable<TEntity>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, Expression<Func<TEntity, bool>> where, bool isDesc = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() => GetPaged(pageIndex, pageSize, sortBy, where, isDesc), cancellationToken);
        }
        #endregion

        #region Updata
        public long Update(TEntity entity)
        {
            _dbSet.Update(entity);
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.SaveChanges();
            }
            return 1;
        }

        public long Update(IQueryable<TEntity> entitys)
        {
            _dbSet.UpdateRange(entitys);
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.SaveChanges();
            }
            return 1;
        }

        public async Task<long> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() => Update(entity), cancellationToken);
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.SaveChanges();
            }
            return 1;
        }

        public async Task<long> UpdateAsync(IQueryable<TEntity> entitys, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() => Update(entitys), cancellationToken);
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.SaveChanges();
            }
            return 1;
        }
        #endregion

    }


}

