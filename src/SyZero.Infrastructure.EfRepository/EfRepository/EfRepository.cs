using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SyZero.Infrastructure.EntityFramework;
using SyZero.Domain.Interface;
using SyZero.Domain.Model;
using System.Threading;
using System.Threading.Tasks;

namespace SyZero.Infrastructure.EfRepository
{
    public class EfRepository<TEntity> : IEfRepository<TEntity> where TEntity : class, IEfEntity
    {
        private readonly SyDbContext _dbContext;
        private DbSet<TEntity> _dbSet;
        public EfRepository(SyDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        #region Count
        public long Count(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.LongCount(where);
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbSet.LongCountAsync(where, cancellationToken);
        }
        #endregion

        #region Insert
        public TEntity Add(TEntity entity)
        {
            var result = _dbSet.Add(entity);
            return result.Entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _dbSet.AddAsync(entity, cancellationToken);
            return result.Entity;
        }

        public int AddList(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            return 0;
        }

        public async Task<int> AddListAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
            return 0;
        }
        #endregion

        #region Delete
        public long Delete(long id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return 0;
            _dbSet.Remove(entity);
            return 0;
        }

        public long Delete(Expression<Func<TEntity, bool>> where)
        {
            var query = GetList(where);
            if (query == null || !query.Any()) return 0;
            _dbSet.RemoveRange(query);
            return 0;
        }

        public async Task<long> DeleteAsync(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return 0;
            _dbSet.Remove(entity);
            return 0;
        }

        public async Task<long> DeleteAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = await GetListAsync(where, cancellationToken);
            if (query == null || !query.Any()) return 0;
            _dbSet.RemoveRange(query);
            return 0;
        } 
        #endregion

        #region Select
        public IEnumerable<TEntity> GetList()
        {
            return _dbSet.AsEnumerable();
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.Where(where).AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() => GetList(), cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() => GetList(where), cancellationToken);
        }

        public TEntity GetModel(long id)
        {
            return _dbSet.Find(id);
        }

        public TEntity GetModel(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.FirstOrDefault(where);
        }

        public async Task<TEntity> GetModelAsync(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbSet.FirstOrDefaultAsync(where, cancellationToken);
        }

        public IEnumerable<TEntity> GetPaged(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, bool isDesc = false)
        {
            if (isDesc)
                return _dbSet.OrderByDescending(sortBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            return _dbSet.OrderBy(sortBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, bool isDesc = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() => GetPaged(pageIndex, pageSize, sortBy, isDesc), cancellationToken);
        }

        public IEnumerable<TEntity> GetPaged(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, Expression<Func<TEntity, bool>> where, bool isDesc = false)
        {
            if (isDesc)
                return _dbSet.Where(where).OrderByDescending(sortBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            return _dbSet.Where(where).OrderBy(sortBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, Expression<Func<TEntity, bool>> where, bool isDesc = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() => GetPaged(pageIndex, pageSize, sortBy, where, isDesc), cancellationToken);
        }
        #endregion

        #region Updata
        public long Update(TEntity entity)
        {
            _dbSet.Update(entity);
            return 0;
        }

        public long Update(IEnumerable<TEntity> entitys)
        {
            _dbSet.UpdateRange(entitys);
            return 0;
        }

        public Task<long> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<long> UpdateAsync(IEnumerable<TEntity> entitys, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

     
        #endregion
    }
}
