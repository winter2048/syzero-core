using System;
using System.Collections.Generic;
using System.Text;
using SyZero.Infrastructure.EntityFramework;
using SyZero.Domain.Interface;

namespace SyZero.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private SyDbContext dataContext;

        public UnitOfWork(SyDbContext _dataContext)
        {
            this.dataContext = _dataContext;
        }

        public void Commit()
        {
            dataContext.SaveChanges();
        }

        public void CommitAsync()
        {
            dataContext.SaveChangesAsync();
        }

    }
}
