using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SyZero.Domain.Interface;
using SyZero.Domain.Model;
using SyZero.Infrastructure.Mongo;

namespace SyZero.Infrastructure.MongoRepository
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : class, IMongoEntity
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
        public long Delete(string id)
        {
            return _collection.DeleteOne(a => a.Id == id).DeletedCount;
        }

        public long Delete(Expression<Func<TEntity, bool>> where)
        {
            return _collection.DeleteOne(where).DeletedCount;
        }

        public async Task<long> DeleteAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
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

        public TEntity GetModel(string id)
        {
            return GetModel(a => a.Id == id);
        }

        public TEntity GetModel(Expression<Func<TEntity, bool>> where)
        {
            var result = _collection.FindSync(where);
            return result.FirstOrDefault();
        }

        public async Task<TEntity> GetModelAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetModelAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _collection.FindAsync(where, cancellationToken: cancellationToken);
            return result.FirstOrDefault(cancellationToken);
        }

        public IEnumerable<TEntity> GetPaged<TProperty>(int pageIndex, int pageSize, Func<TEntity, TProperty> sortBy, bool isDesc = false)
        {
            if (isDesc)
                return _collection.AsQueryable().OrderByDescending(sortBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            return _collection.AsQueryable().OrderBy(sortBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        public IEnumerable<TEntity> GetPaged<TProperty>(int pageIndex, int pageSize, Func<TEntity, TProperty> sortBy, Expression<Func<TEntity, bool>> where, bool isDesc = false)
        {
            if (isDesc)
                return _collection.AsQueryable().Where(where).OrderByDescending(sortBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            return _collection.AsQueryable().Where(where).OrderBy(sortBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        public Task<IEnumerable<TEntity>> GetPagedAsync<TProperty>(int pageIndex, int pageSize, Func<TEntity, TProperty> sortBy, bool isDesc = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => GetPaged(pageIndex, pageSize, sortBy, isDesc), cancellationToken);
        }

        public Task<IEnumerable<TEntity>> GetPagedAsync<TProperty>(int pageIndex, int pageSize, Func<TEntity, TProperty> sortBy, Expression<Func<TEntity, bool>> where, bool isDesc = false, CancellationToken cancellationToken = default(CancellationToken))
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
        #endregion
    }
}
