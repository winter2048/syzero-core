using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Infrastructure.Repository
{
    public interface IUnitOfWork
    {
        void Commit();
        void CommitAsync();
    }
}
