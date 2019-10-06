using System;
using System.Collections.Generic;
using System.Text;
using SyZero.Domain.Entities;
using SyZero.Domain.Repository;

namespace SyZero.Domain.Repository
{
    public interface IRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
      
    }
    public interface IRepository<TEntity> : IBaseRepository<TEntity, long> where TEntity : class, IEntity<long>
    {

    }
}
