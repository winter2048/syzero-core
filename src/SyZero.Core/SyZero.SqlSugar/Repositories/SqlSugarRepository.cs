using SqlSugar;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SyZero.Domain.Entities;
using SyZero.Domain.Repository;
using SyZero.SqlSugar.DbContext;

namespace SyZero.SqlSugar.Repositories
{
    public class SqlSugarRepository<TDbContext, TEntity> : IRepository<TEntity>
      where TEntity : class, IEntity, new()
      where TDbContext : SyZeroDbContext
    {
        protected TDbContext _dbContext;
        protected SimpleClient<TEntity> _dbSet;

        public SqlSugarRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = new SimpleClient<TEntity>(_dbContext);
        }

        #region Count
        public int Count(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.Count(where);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbSet.CountAsync(where);
        }
        #endregion

        #region Insert
        public TEntity Add(TEntity entity)
        {
            var result = _dbSet.Insert(entity);
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _dbSet.InsertAsync(entity);
            return entity;
        }

        public int AddList(IQueryable<TEntity> entities)
        {
            var result = _dbSet.InsertRange(entities.ToList());
            return 1;
        }

        public async Task<int> AddListAsync(IQueryable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _dbSet.InsertRangeAsync(entities.ToList());
            return 1;
        }
        #endregion

        #region Delete
        public long Delete(long id)
        {
            var result = _dbSet.DeleteById(id);
            return 1;
        }

        public long Delete(Expression<Func<TEntity, bool>> where)
        {
            var result = _dbSet.Delete(where);
            return 1;
        }

        public async Task<long> DeleteAsync(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _dbSet.DeleteByIdAsync(id);
            return 1;
        }

        public async Task<long> DeleteAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _dbSet.DeleteAsync(where);
            return 1;
        }
        #endregion

        #region Select
        public IQueryable<TEntity> GetList()
        {
            return _dbSet.GetList().AsQueryable();
        }

        public IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.GetList(where).AsQueryable();
        }

        public async Task<IQueryable<TEntity>> GetListAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var list = await _dbSet.GetListAsync();
            return list.AsQueryable();
        }

        public async Task<IQueryable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            var list = await _dbSet.GetListAsync(where);
            return list.AsQueryable();
        }

        public TEntity GetModel(long id)
        {
            return _dbSet.GetById(id);
        }

        public async Task<TEntity> GetModelAsync(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbSet.GetByIdAsync(id);
        }

        public TEntity GetModel(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.GetSingle(where);
        }

        public async Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbSet.GetSingleAsync(where);
        }

        public IQueryable<TEntity> GetPaged(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, bool isDesc = false)
        {
            return _dbSet.GetPageList(p => true, new PageModel()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            }, sortBy, isDesc ? OrderByType.Desc : OrderByType.Asc).AsQueryable();
        }

        public async Task<IQueryable<TEntity>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, bool isDesc = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await _dbSet.GetPageListAsync(p => true, new PageModel()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            }, sortBy, isDesc ? OrderByType.Desc : OrderByType.Asc)).AsQueryable();
        }

        public IQueryable<TEntity> GetPaged(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, Expression<Func<TEntity, bool>> where, bool isDesc = false)
        {
            return _dbSet.GetPageList(where, new PageModel()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            }, sortBy, isDesc ? OrderByType.Desc : OrderByType.Asc).AsQueryable();
        }

        public async Task<IQueryable<TEntity>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, Expression<Func<TEntity, bool>> where, bool isDesc = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await _dbSet.GetPageListAsync(where, new PageModel()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            }, sortBy, isDesc ? OrderByType.Desc : OrderByType.Asc)).AsQueryable();
        }
        #endregion

        #region Updata
        public long Update(TEntity entity)
        {
            return _dbSet.Update(entity) ? 1 : 0;
        }

        public long Update(IQueryable<TEntity> entitys)
        {
            return _dbSet.UpdateRange(entitys.ToList()) ? 1 : 0;
        }

        public async Task<long> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await _dbSet.UpdateAsync(entity)) ? 1 : 0;
        }

        public async Task<long> UpdateAsync(IQueryable<TEntity> entitys, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await _dbSet.UpdateRangeAsync(entitys.ToList())) ? 1 : 0;
        }
        #endregion

    }



}

