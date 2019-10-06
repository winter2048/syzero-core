using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SyZero.Domain.Repository;
using SyZero.Domain.Entities;

namespace SyZero.MongoDB
{
    public class MongoRepository<TEntity> : IRepository<TEntity,long> where TEntity : class, IEntity<long>
    {
        private readonly IMongoContext _Context;
        private IMongoCollection<TEntity> _collection;
        public MongoRepository(IMongoContext context)
        {
            _Context = context;
            _collection = _Context.Set<TEntity>();
        }
 

        #region Insert
        public TEntity Add(TEntity entity)
        {
            _collection.InsertOne(entity);
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
            return entity;
        }

        public int AddList(IEnumerable<TEntity> entities)
        {
            _collection.InsertMany(entities);
            return entities.Count();
        }

        public async Task<int> AddListAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _collection.InsertManyAsync(entities, cancellationToken: cancellationToken);
            return entities.Count();
        }
        #endregion

        #region Count
        public long Count(Expression<Func<TEntity, bool>> where)
        {
            return _collection.Count(where);
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _collection.CountAsync(where, null, cancellationToken);
        } 
        #endregion

        #region Delete
        public long Delete(long id)
        {
            return _collection.DeleteOne(a => a.Id == id).DeletedCount;
        }

        public long Delete(Expression<Func<TEntity, bool>> where)
        {
            return _collection.DeleteOne(where).DeletedCount;
        }

        public async Task<long> DeleteAsync(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _collection.DeleteOneAsync(a => a.Id == id, cancellationToken);
            return result.DeletedCount;
        }

        public async Task<long> DeleteAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _collection.DeleteOneAsync(where, cancellationToken);
            return result.DeletedCount;
        } 
        #endregion

        #region Select
        public IEnumerable<TEntity> GetList()
        {
            return _collection.AsQueryable();
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> where)
        {
            return _collection.FindSync(where).ToEnumerable();
        }

        public Task<IEnumerable<TEntity>> GetListAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => GetList(), cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            var reulst = await _collection.FindAsync(where, cancellationToken: cancellationToken);
            return reulst.ToEnumerable();
        }

        public TEntity GetModel(long id)
        {
            return GetModel(a => a.Id == id);
        }

        public TEntity GetModel(Expression<Func<TEntity, bool>> where)
        {
            var result = _collection.FindSync(where);
            return result.FirstOrDefault();
        }

     

        public async Task<TEntity> GetModelAsync(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetModelAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _collection.FindAsync(where, cancellationToken: cancellationToken);
            return result.FirstOrDefault(cancellationToken);
        }

        public IEnumerable<TEntity> GetPaged(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, bool isDesc = false)
        {
            SortDefinition<TEntity> sorts = new ObjectSortDefinition<TEntity>(new { });
            if (isDesc)
                sorts = sorts.Descending(sortBy);
            else
                sorts = sorts.Ascending(sortBy);
            return _collection.Find(p=>true).Sort(sorts).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToEnumerable();
        }

        public IEnumerable<TEntity> GetPaged(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, Expression<Func<TEntity, bool>> where, bool isDesc = false)
        {
            SortDefinition<TEntity> sorts = new ObjectSortDefinition<TEntity>(new { });
            if (isDesc)
                sorts = sorts.Descending(sortBy);
            else
                sorts = sorts.Ascending(sortBy);
            return _collection.Find(where).Sort(sorts).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToEnumerable();
        }

        public Task<IEnumerable<TEntity>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, bool isDesc = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => GetPaged(pageIndex, pageSize, sortBy, isDesc), cancellationToken);
        }

        public Task<IEnumerable<TEntity>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, Expression<Func<TEntity, bool>> where, bool isDesc = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => GetPaged(pageIndex, pageSize, sortBy, where, isDesc), cancellationToken);
        }


        #endregion

        #region Updata
        public long Update(TEntity entity)
        {
            var doc = entity.ToBsonDocument();
            var result = _collection.UpdateOne(Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id),
                new BsonDocumentUpdateDefinition<TEntity>(doc));
            return result.ModifiedCount;
        }

        public long Update(IEnumerable<TEntity> entitys)
        {
            long reulst = 0;
            foreach (var item in entitys)
                reulst += Update(item);
            return reulst;
        }

        public async Task<long> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var doc = entity.ToBsonDocument();
            var result = await _collection.UpdateOneAsync(Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id),
                new BsonDocumentUpdateDefinition<TEntity>(doc), cancellationToken: cancellationToken);
            return result.ModifiedCount;
        }

        public async Task<long> UpdateAsync(IEnumerable<TEntity> entitys, CancellationToken cancellationToken = default(CancellationToken))
        {
            long reulst = 0;
            foreach (var item in entitys)
                reulst += await UpdateAsync(item, cancellationToken);
            return reulst;
        }

        TEntity IBaseRepository<TEntity, long>.Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        Task<TEntity> IBaseRepository<TEntity, long>.AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        int IBaseRepository<TEntity, long>.AddList(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        Task<int> IBaseRepository<TEntity, long>.AddListAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        long IBaseRepository<TEntity, long>.Count(Expression<Func<TEntity, bool>> where)
        {
            throw new NotImplementedException();
        }

        Task<long> IBaseRepository<TEntity, long>.CountAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        int IBaseRepository<TEntity, long>.Delete(long id)
        {
            throw new NotImplementedException();
        }

        int IBaseRepository<TEntity, long>.Delete(Expression<Func<TEntity, bool>> where)
        {
            throw new NotImplementedException();
        }

        Task<int> IBaseRepository<TEntity, long>.DeleteAsync(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<int> IBaseRepository<TEntity, long>.DeleteAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        IEnumerable<TEntity> IBaseRepository<TEntity, long>.GetList()
        {
            throw new NotImplementedException();
        }

        IEnumerable<TEntity> IBaseRepository<TEntity, long>.GetList(Expression<Func<TEntity, bool>> where)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TEntity>> IBaseRepository<TEntity, long>.GetListAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TEntity>> IBaseRepository<TEntity, long>.GetListAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        TEntity IBaseRepository<TEntity, long>.GetModel(long id)
        {
            throw new NotImplementedException();
        }

        TEntity IBaseRepository<TEntity, long>.GetModel(Expression<Func<TEntity, bool>> where)
        {
            throw new NotImplementedException();
        }

        Task<TEntity> IBaseRepository<TEntity, long>.GetModelAsync(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<TEntity> IBaseRepository<TEntity, long>.GetModelAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        IEnumerable<TEntity> IBaseRepository<TEntity, long>.GetPaged(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, bool isDesc)
        {
            throw new NotImplementedException();
        }

        IEnumerable<TEntity> IBaseRepository<TEntity, long>.GetPaged(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, Expression<Func<TEntity, bool>> where, bool isDesc)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TEntity>> IBaseRepository<TEntity, long>.GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, bool isDesc, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TEntity>> IBaseRepository<TEntity, long>.GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortBy, Expression<Func<TEntity, bool>> where, bool isDesc, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        int IBaseRepository<TEntity, long>.Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        int IBaseRepository<TEntity, long>.Update(IEnumerable<TEntity> entitys)
        {
            throw new NotImplementedException();
        }

        Task<int> IBaseRepository<TEntity, long>.UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<int> IBaseRepository<TEntity, long>.UpdateAsync(IEnumerable<TEntity> entitys, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
