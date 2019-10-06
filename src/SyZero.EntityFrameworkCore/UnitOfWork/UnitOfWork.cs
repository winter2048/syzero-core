using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SyZero.Domain.Repository;
using System.Threading.Tasks;

namespace SyZero.EntityFrameworkCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private SyDbContext dataContext;

        public UnitOfWork(SyDbContext _dataContext)
        {
            this.dataContext = _dataContext;
        }

        public async Task<int> SaveAsyncChange(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default(CancellationToken))
        {
           return await dataContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public int SaveChange(bool acceptAllChangesOnSuccess = true)
        {
            return dataContext.SaveChanges(acceptAllChangesOnSuccess);
        }
    }
}
