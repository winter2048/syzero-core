using System;
using System.Collections.Generic;
using System.Text;
using SyZero.Domain.Model;

namespace SyZero.Domain.Interface
{
    public interface IMongoRepository<TEntity> : IBaseRepository<TEntity, string>
        where TEntity : class, IMongoEntity
    {
        #region Mongo代码
        //TODU
        #endregion
    }
}
