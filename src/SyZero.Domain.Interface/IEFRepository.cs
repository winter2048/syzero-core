using System;
using System.Collections.Generic;
using System.Text;
using SyZero.Domain.Model;

namespace SyZero.Domain.Interface
{
    public interface IEfRepository<TEntity> : IBaseRepository<TEntity, long>
    where TEntity : class, IEfEntity
    {
        #region EF仓储代码
        //TODU
        #endregion



    }
}
