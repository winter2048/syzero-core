using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SyZero.Domain.Repository;

namespace SyZero.EntityFrameworkCore
{
    public class UnitOfWork<TDbContext> : IUnitOfWork
         where TDbContext : DbContext
    {
        private TDbContext dataContext;

        public UnitOfWork(TDbContext _dataContext)
        {
            this.dataContext = _dataContext;
        }

        public void BeginTransaction()
        {
            if (dataContext.Database.CurrentTransaction == null)
            {
                dataContext.Database.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            if (dataContext.Database.CurrentTransaction != null)
            {
                dataContext.Database.CurrentTransaction.Commit();
            }
        }

        public void RollbackTransaction()
        {
            if (dataContext.Database.CurrentTransaction != null)
            {
                dataContext.Database.CurrentTransaction.Rollback();
            }
        }

        public void DisposeTransaction()
        {
            if (dataContext.Database.CurrentTransaction != null)
            {
                dataContext.Database.CurrentTransaction.Dispose();
            }
        }

        public async Task BeginTransactionAsync()
        {
            if (dataContext.Database.CurrentTransaction == null)
            {
                await dataContext.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitTransactionAsync()
        {
            if (dataContext.Database.CurrentTransaction != null)
            {
                await dataContext.Database.CurrentTransaction.CommitAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (dataContext.Database.CurrentTransaction != null)
            {
                await dataContext.Database.CurrentTransaction.RollbackAsync();
            }
        }

        public async Task DisposeTransactionAsync()
        {
            if (dataContext.Database.CurrentTransaction != null)
            {
                await dataContext.Database.CurrentTransaction.DisposeAsync();
            }
        }


    }
}
