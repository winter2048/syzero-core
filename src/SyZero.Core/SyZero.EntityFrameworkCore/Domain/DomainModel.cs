using Microsoft.EntityFrameworkCore;
using SyZero.Domain.Entities;
using SyZero.Domain.Model;
using SyZero.EntityFrameworkCore.Repositories;
using SyZero.Util;

namespace SyZero.EntityFrameworkCore.Domain
{
    /// <summary>
    /// 领域模型
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="IEntity"></typeparam>
    public class DomainModel<TDbContext, TEntity> : EfRepository<TDbContext, TEntity>, IDomainModel<TEntity>
        where TEntity : class, IEntity
        where TDbContext : DbContext
    {

        public DomainModel()
        {
            _dbContext = AutofacUtil.GetScopeService<TDbContext>();
            _dbSet = _dbContext.Set<TEntity>();
        }

    }
}
