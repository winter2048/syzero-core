using System.Threading;
using System.Threading.Tasks;
using SyZero.Domain.Repository;
using SyZero.SqlSugar.DbContext;

namespace SyZero.SqlSugar
{
    public class UnitOfWork<TDbContext> : IUnitOfWork
         where TDbContext : SyZeroDbContext
    {
        private TDbContext dataContext;

        public UnitOfWork(TDbContext _dataContext)
        {
            this.dataContext = _dataContext;
        }

        public void BeginTransaction()
        {
            this.dataContext.BeginTran();
        }

        public void CommitTransaction()
        {
            this.dataContext.CommitTran();
        }


        public void RollbackTransaction()
        {
            this.dataContext.RollbackTran();
        }

        public void DisposeTransaction()
        {
            this.dataContext.Dispose();
        }

        public async Task BeginTransactionAsync()
        {
            await Task.Run(this.dataContext.Context.Ado.BeginTran);
        }

        public async Task CommitTransactionAsync()
        {
            await Task.Run(this.dataContext.Context.Ado.CommitTran);
        }

        public async Task RollbackTransactionAsync()
        {
            await Task.Run(this.dataContext.Context.Ado.RollbackTran);
        }

        public async Task DisposeTransactionAsync()
        {
            await Task.Run(this.dataContext.Context.Ado.Dispose);
        }


    }
}
